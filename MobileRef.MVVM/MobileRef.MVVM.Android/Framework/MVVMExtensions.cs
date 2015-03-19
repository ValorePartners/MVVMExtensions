using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;


namespace MobileRef.MVVM.Android
{
	public class LinkerPrepare
	{
		public static bool AlreadyPrepared{ get; set; }

		public static void Init (Context ctx)
		{
			if (!AlreadyPrepared) {
				EditText et = new EditText (ctx);
				et.TextChanged += null;
				et.Text = "";
				var x = et.Text;

				TextView tv = new TextView (ctx);
				tv.Text = null;
				x = tv.Text;

				Button b = new Button (ctx);
				b.Click += null;
				b.LongClick += null;
				b.Text = "";
				x = b.Text;

				ImageButton ib = new ImageButton (ctx);
				ib.Click += null;
				ib.LongClick += null;
				x = b.Text;

				ListView lv = new ListView (ctx);
				lv.ItemSelected += null;
				lv.ItemClick += null;
				lv.LongClick += null;

				AlreadyPrepared = true;
			}
		}
	}

	[Preserve (AllMembers = true)]
	public class PropertyBindingSettings
	{
		public int ResourceId {
			get;
			set;
		}

		public string BindingObject {
			get;
			set;
		}

		public string BindingProperty {
			get;
			set;
		}

		public string ViewModelProperty {
			get;
			set;
		}

		public string EventName {
			get;
			set;
		}

		public string Format {
			get;
			set;
		}

	}

	[Preserve (AllMembers = true)]
	public class EventBindingSettings
	{
		public int ResourceId {
			get;
			set;
		}

		public string BindingObject {
			get;
			set;
		}

		public string EventName {
			get;
			set;
		}

		public string ViewModelCommand {
			get;
			set;
		}
	}


	[Preserve (AllMembers = true)]
	public class BindingManager<T,K> 
		where T: class, IHandlers
		where K:INotifyPropertyChanged
	{
		private List<PropertyBindingSettings> pbs;
		private List<EventBindingSettings> ebs;
		private K viewModel;
		private T bindingObj;

		public void BindProperty (string bindingObject, string bindingProperty, string eventName, string viewModelProperty, string format)
		{
			pbs.Add (new PropertyBindingSettings () {
				BindingObject = bindingObject,
				BindingProperty = bindingProperty,
				ViewModelProperty = viewModelProperty,
				EventName = eventName,
				ResourceId = ControlUtil.GetResourceId (bindingObject),
				Format = format
			});
		}

		public void BindProperty (string bindingObject, string bindingProperty, string eventName, string viewModelProperty)
		{
			BindProperty (bindingObject, bindingProperty, eventName, viewModelProperty, null);
		}

		public void BindProperty (Expression<Func<object>> bindingProperty, Expression<Func<object>> viewModelProperty)
		{
			BindProperty (bindingProperty, viewModelProperty, null);
		}

		public void BindProperty (Expression<Func<object>> bindingProperty, Expression<Func<object>> viewModelProperty, string format)
		{
			string eventName = null;
			var vmFieldName = GetPropertyName (viewModelProperty);
			var controlName = GetControlName (bindingProperty);
			var bpFieldName = GetPropertyName (bindingProperty);
			var bpName = typeof(T).GetProperty (controlName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).PropertyType.Name;
			switch (bpName) {
			case "EditText":	
				eventName = "TextChanged";
				break;
			case "CheckBox":
				eventName = "CheckedChange";
				break;
			case "ListView":
				if (bpFieldName == "SelectedItem")
					eventName = "ItemClick";
				break;
			}
			BindProperty (controlName, bpFieldName, eventName, vmFieldName, format);
		}

		public void BindCommand (string bindingObject, string eventName, string viewModelCommand)
		{
			ebs.Add (new EventBindingSettings () {
				BindingObject = bindingObject,
				EventName = eventName,
				ViewModelCommand = viewModelCommand,
				ResourceId = ControlUtil.GetResourceId (bindingObject)
			});
		}

		public void BindCommand (Expression<Func<object>> bindingProperty, Expression<Func<object>> viewModelProperty)
		{
			string eventName = null;
			var controlName = GetCommandControlName (bindingProperty);
			var bpName = typeof(T).GetProperty (controlName,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).PropertyType.Name;
			var vmFieldName = GetPropertyName (viewModelProperty);
			switch (bpName) {
			case "Button":	
				eventName = "Click";
				break;
			case "LinearLayout":	
				eventName = "Click";
				break;
			case "RelativeLayout":	
				eventName = "Click";
				break;
			case "ImageButton":	
				eventName = "Click";
				break;
			case "RadioGroup":	
				eventName = "CheckedChange";
				break;
			case "ListView":	
				eventName = "ItemClick";
				break;
			case "Spinner":
				eventName = "ItemSelected";
				break;
			}

			BindCommand (controlName, eventName, vmFieldName);
		}

		public void RegisterBindingEvents (T bindingObj, K viewModel, bool registerDefaultControls = true)
		{
			this.bindingObj = bindingObj;
			this.viewModel = viewModel;
			foreach (var obj in pbs) {
				ConnectPropertyBindingSetting (obj);
			}
			foreach (var obj in ebs) {
				ConnectCommandBindingSetting (obj);
			}

			this.viewModel.PropertyChanged += this.InternalViewModelEventHandler;
			foreach (var vmProp in this.viewModel.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var ii = vmProp.PropertyType.GetInterface ("INotifyCollectionChanged");
				if (ii != null) {
					var collection = (INotifyCollectionChanged)vmProp.GetValue (this.viewModel);
					collection.CollectionChanged += this.InternalViewModelEventHandler;
				}
			}

			if (registerDefaultControls) {
				bindingObj.RegisterEvents (this.ebs);
			}

		}

		public void UnRegisterBindingEvents (T bindingObject,bool unRegisterDefaultControls = true)
		{
			foreach (var obj in pbs) {
				DisconnectPropertyBindingSetting (obj);
			}
			foreach (var obj in ebs) {
				DisconnectCommandBindingSetting (obj);
			}

			this.viewModel.PropertyChanged -= this.InternalViewModelEventHandler;
			foreach (var vmProp in this.viewModel.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var ii = vmProp.PropertyType.GetInterface ("INotifyCollectionChanged");
				if (ii != null) {
					var collection = (INotifyCollectionChanged)vmProp.GetValue (this.viewModel);
					collection.CollectionChanged -= this.InternalViewModelEventHandler;
				}
			}

			if (unRegisterDefaultControls) {
				bindingObject.UnRegisterEvents ();
			}

		}

		public BindingManager ()
		{
			pbs = new List<PropertyBindingSettings> ();
			ebs = new List<EventBindingSettings> ();
		}

		private void InternalControlEventHandler (object sender, EventArgs args)
		{
			var ctrlId = ((global::Android.Views.View)sender).Id;
			var ctrl = pbs.FirstOrDefault (x => x.ResourceId == ctrlId);

			if (ctrl != null) {
				var prop = sender.GetType ().GetProperty (ctrl.BindingProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
				var propValue = prop.GetValue (sender, null);
				var vmProp = viewModel.GetType ().GetProperty (ctrl.ViewModelProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

				if (prop.ReflectedType.Name == "ListView") {
					if (ctrl.BindingProperty == "SelectedItem") {
						if (args is global::Android.Widget.AdapterView.ItemClickEventArgs) {
							var position = ((global::Android.Widget.AdapterView.ItemClickEventArgs)args).Position;
							var item = ((ListView)sender).Adapter.GetItem (position);
							var vmObj = ConvertToPOCO (item, vmProp.PropertyType);
							vmProp.SetValue (viewModel, vmObj);
						}
					}

				} else {
					var cvPropValue = ValueConverter.ChangeType (vmProp.PropertyType.Name, propValue.ToString ());
					if (cvPropValue == null)
						return;
					vmProp.SetValue (viewModel, cvPropValue);
				}
			}
		}

		private void InternalCommandEventHandler (object sender, EventArgs args)
		{
			var ctrlId = ((global::Android.Views.View)sender).Id;
			var ctrl = ebs.FirstOrDefault (x => x.ResourceId == ctrlId);

			if (ctrl != null) {
				var vmProp = typeof(K).GetProperty (ctrl.ViewModelCommand,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
				var vmCommand = (ICommand)vmProp.GetValue (this.viewModel, null);
				vmCommand.Execute (null);
			}
		}

		private void InternalViewModelEventHandler (object sender, EventArgs args)
		{
			if (args is NotifyCollectionChangedEventArgs) {
				var props = typeof(K).GetProperties (BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
				foreach (var prop in props) {
					var ii = prop.PropertyType.GetInterface ("INotifyCollectionChanged");
					if (ii != null) {
						if (sender == prop.GetValue (viewModel)) {
							var propName = prop.Name;
							foreach (var obj in pbs.Where(x => x.ViewModelProperty == propName)) {
								if (!string.IsNullOrEmpty (obj.EventName))
									DisconnectPropertyBindingSetting (obj);

								var ctrlProp = typeof(T).GetProperty (obj.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
								var ib = ctrlProp.PropertyType.GetInterface ("IBaseAdapter");
								if (ib != null) {
									var ctrlInstance = ctrlProp.GetValue (bindingObj);
									((IBaseAdapter)ctrlInstance).UpdateCollection ((ICollection)sender);
								}

								if (!string.IsNullOrEmpty (obj.EventName))
									ConnectPropertyBindingSetting (obj);
							}
						}
					}
				}
			}
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				foreach (var obj in pbs.Where(x => x.ViewModelProperty == propName)) {
					if (!string.IsNullOrEmpty (obj.EventName))
						DisconnectPropertyBindingSetting (obj);
					var ctrlProp = typeof(T).GetProperty (obj.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var ctrlInstance = ctrlProp.GetValue (bindingObj);
					var ctrlInstaceProp = ctrlInstance.GetType ().GetProperty (obj.BindingProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var currentValue = ctrlInstaceProp.GetValue (ctrlInstance);
					var vmProp = sender.GetType ().GetProperty (obj.ViewModelProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var	vmValue = vmProp.GetValue (sender, null);
		
					if (ctrlProp.PropertyType.Name == "ListView") {
						if (obj.BindingProperty != "SelectedItem") {

						}
					} else if (ctrlProp.PropertyType.GetInterface ("IAdapter") != null) {
						var ib = ctrlProp.PropertyType.GetInterface ("IBaseAdapter");
						if (ib != null) {
							((IBaseAdapter)ctrlInstance).UpdateCollection ((ICollection)vmValue);
						}
					} else if(currentValue!=null && currentValue.GetType().Name!="String" && (currentValue.GetType().Name==vmValue.GetType().Name)){
						ctrlInstaceProp.SetValue (ctrlInstance, vmValue);
					}
					else {
						if (currentValue==null || !currentValue.Equals (vmValue)) {
							if (!string.IsNullOrEmpty (obj.Format)) {
								var formattedValue = string.Format (obj.Format, vmValue);
								ctrlInstaceProp.SetValue (ctrlInstance, formattedValue);
							} else {
								ctrlInstaceProp.SetValue (ctrlInstance, vmValue.ToString ());
							}
							if (ctrlProp.PropertyType.Name == "EditText") {
								int textLength = ((EditText)ctrlInstance).Text.Length;
								((EditText)ctrlInstance).SetSelection (textLength, textLength);
							}
						}
					}
					if (!string.IsNullOrEmpty (obj.EventName))
						ConnectPropertyBindingSetting (obj);
				}
			}
		}



		private void ConnectPropertyBindingSetting (PropertyBindingSettings setting)
		{
			var prop = typeof(T).GetProperty (setting.BindingObject,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (bindingObj);
					var eventInstance = instance.GetType ().GetEvent (setting.EventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = this.GetType ().GetMethod ("InternalControlEventHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetAddMethod ();
							var d = Delegate.CreateDelegate (tDelegate, this, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (instance, addHandlerArgs);
						}
					}
				}
			}
		}

		private void ConnectCommandBindingSetting (EventBindingSettings setting)
		{
			var prop = typeof(T).GetProperty (setting.BindingObject,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (bindingObj);
					var eventInstance = instance.GetType ().GetEvent (setting.EventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = this.GetType ().GetMethod ("InternalCommandEventHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetAddMethod ();
							var d = Delegate.CreateDelegate (tDelegate, this, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (instance, addHandlerArgs);
						}
					}
				}
			}
		}

		private void DisconnectPropertyBindingSetting (PropertyBindingSettings setting)
		{
			var prop = typeof(T).GetProperty (setting.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (bindingObj);
					var eventInstance = instance.GetType ().GetEvent (setting.EventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = this.GetType ().GetMethod ("InternalControlEventHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetRemoveMethod ();
							var d = Delegate.CreateDelegate (tDelegate, this, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (instance, addHandlerArgs);
						}
					}
				}
			}
		}

		private void DisconnectCommandBindingSetting (EventBindingSettings setting)
		{
			var prop = typeof(T).GetProperty (setting.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (bindingObj);
					var eventInstance = instance.GetType ().GetEvent (setting.EventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = this.GetType ().GetMethod ("InternalCommandEventHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetRemoveMethod ();
							var d = Delegate.CreateDelegate (tDelegate, this, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (instance, addHandlerArgs);
						}
					}
				}
			}
		}

		private string GetControlName (Expression<Func<object>> propertyRefExpr)
		{

			if (propertyRefExpr.Body is MemberExpression) {
				var exp = ((MemberExpression)propertyRefExpr.Body);
				return ((MemberExpression)exp.Expression).Member.Name;
			} else {
				var unaryExpr = propertyRefExpr.Body as UnaryExpression;
				var memberExpr = unaryExpr.Operand as MemberExpression;
				return ((MemberExpression)memberExpr.Expression).Member.Name;

			}
		}

		private string GetCommandControlName (Expression<Func<object>> propertyRefExpr)
		{
			var exp = ((MemberExpression)propertyRefExpr.Body);
			return ((MemberExpression)exp).Member.Name;
		}

		private string GetPropertyName (Expression<Func<object>> propertyRefExpr)
		{
			if (propertyRefExpr == null)
				throw new ArgumentNullException ("propertyRefExpr", "propertyRefExpr is null.");

			var memberExpr = propertyRefExpr.Body as MemberExpression;
			if (memberExpr == null) {
				var unaryExpr = propertyRefExpr.Body as UnaryExpression;
				if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
					memberExpr = unaryExpr.Operand as MemberExpression;
			}

			if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
				return memberExpr.Member.Name;

			throw new ArgumentException ("No property reference expression was found.",
				"propertyRefExpr");
		}

		private object ConvertToPOCO (Java.Lang.Object obj, Type convertTo)
		{
			var javaObj = obj.GetType ().GetProperty ("Instance");
			var javaBlank = javaObj.GetValue (obj, null);
			return Convert.ChangeType (javaBlank, convertTo);
		}

	}

	public static class FrameworkExtensions
	{

		public static void RegisterEvents<T> (this T obj) 
			where T: class, IHandlers
		{
			obj.RegisterEvents (new List<EventBindingSettings> ());
		}
			
			
		public static void RegisterEvents<T> (this T obj, List<EventBindingSettings> ebs) 
			where T: class, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;

				if (ControlUtil.ActionDictionary.ContainsKey (pt.Name)) {
					var eventName = ControlUtil.ActionDictionary [pt.Name];
					if (!ebs.Any (x => x.BindingObject == prop.Name && x.EventName == eventName)) {
						ControlUtil.HookUIControl (prop.GetValue (obj), obj);
					}
				}

				var ce = prop.GetCustomAttribute (typeof(RegisterEvent));
				if (ce != null) {
					var ceObj = (RegisterEvent)ce;
					foreach (var eventName in ceObj.EventNames) {
						var control = prop.GetValue (obj);
						var evClick = control.GetType ().GetEvent (eventName);
						if (evClick != null) {
							var tDelegate = evClick.EventHandlerType;
							var miHandler = typeof(T).GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
							if (miHandler != null) {
								var addHandler = evClick.GetAddMethod ();
								var d = Delegate.CreateDelegate (tDelegate, obj, miHandler);
								System.Object[] addHandlerArgs = { d };
								addHandler.Invoke (control, addHandlerArgs);
							}
						}
					}
				}
				var i = prop.PropertyType.GetInterface ("INotifyPropertyChanged");
				if (i != null) {
					var vm = (INotifyPropertyChanged)prop.GetValue (obj);
					vm.PropertyChanged += iEvent.ViewEventHandler;
					foreach (var vmProp in vm.GetType().GetProperties()) {
						var ii = vmProp.PropertyType.GetInterface ("INotifyCollectionChanged");
						if (ii != null) {
							var collection = (INotifyCollectionChanged)vmProp.GetValue (vm);
							collection.CollectionChanged += iEvent.ViewEventHandler;
						}
					}
				}
			}
		}
			

		public static void UnRegisterEvents<T> (this T obj) 
			where T: class, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;

				if (ControlUtil.ActionDictionary.ContainsKey (pt.Name)) {
					ControlUtil.UnHookUIControll (prop.GetValue (obj), obj);
				}
				var ce = prop.GetCustomAttribute (typeof(RegisterEvent));
				if (ce != null) {
					var ceObj = (RegisterEvent)ce;
					foreach (var eventName in ceObj.EventNames) {
						var control = prop.GetValue (obj);
						var evClick = control.GetType ().GetEvent (eventName);
						if (evClick != null) {
							var tDelegate = evClick.EventHandlerType;
							var miHandler = typeof(T).GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
							if (miHandler != null) {
								var addHandler = evClick.GetRemoveMethod ();
								var d = Delegate.CreateDelegate (tDelegate, obj, miHandler);
								System.Object[] addHandlerArgs = { d };
								addHandler.Invoke (control, addHandlerArgs);
							}
						}
					}
				}
				var i = prop.PropertyType.GetInterface ("INotifyPropertyChanged");
				if (i != null) {
					var vm = (INotifyPropertyChanged)prop.GetValue (obj);
					vm.PropertyChanged -= iEvent.ViewEventHandler;
					foreach (var vmProp in vm.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
						var ii = vmProp.PropertyType.GetInterface ("INotifyCollectionChanged");
						if (ii != null) {
							var collection = (INotifyCollectionChanged)vmProp.GetValue (vm);
							collection.CollectionChanged -= iEvent.ViewEventHandler;
						}
					}
				}
			}
		}

		public static void CreateControls<T> (this T obj) where T: Activity
		{
			if (obj != null) {
				foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
					var pn = prop.Name;
					var pt = prop.PropertyType;
					switch (pt.Name) {
					case "Button":
						var buttonId = ControlUtil.GetResourceId (pn);
						var button = obj.FindViewById<Button> (buttonId);
						prop.SetValue (obj, button);
						break;
					case "ListView":
						var listViewId = ControlUtil.GetResourceId (pn);
						var listView = obj.FindViewById<ListView> (listViewId);
						prop.SetValue (obj, listView);
						break;
					case "ScrollView":
						var scrollViewId = ControlUtil.GetResourceId (pn);
						var scrollView = obj.FindViewById<ScrollView> (scrollViewId);
						prop.SetValue (obj, scrollView);
						break;
					case "LinearLayout":
						var linearLayoutId = ControlUtil.GetResourceId (pn);
						var linearLayout = obj.FindViewById<LinearLayout> (linearLayoutId);
						prop.SetValue (obj, linearLayout);
						break;
					case "RelativeLayout":
						var relativeLayoutId = ControlUtil.GetResourceId (pn);
						var relativeLayout = obj.FindViewById<RelativeLayout> (relativeLayoutId);
						prop.SetValue (obj, relativeLayout);
						break;
					case "TextView":
						var textViewId = ControlUtil.GetResourceId (pn);
						var textView = obj.FindViewById<TextView> (textViewId);
						prop.SetValue (obj, textView);
						break;
					case "EditText":
						var editTextId = ControlUtil.GetResourceId (pn);
						var editText = obj.FindViewById<EditText> (editTextId);
						prop.SetValue (obj, editText);
						break;
					case "Spinner":
						var spinnerId = ControlUtil.GetResourceId (pn);
						var spinner = obj.FindViewById<Spinner> (spinnerId);
						prop.SetValue (obj, spinner);
						break;
					case "ImageView":
						var imageViewId = ControlUtil.GetResourceId (pn);
						var imageView = obj.FindViewById<ImageView> (imageViewId);
						prop.SetValue (obj, imageView);
						break;	
					case "ImageButton":
						var imageButtonId = ControlUtil.GetResourceId (pn);
						var imageButton = obj.FindViewById<ImageButton> (imageButtonId);
						prop.SetValue (obj, imageButton);
						break;	
					case "RadioGroup":
						var radioGroupId = ControlUtil.GetResourceId (pn);
						var radioGroup = obj.FindViewById<RadioGroup> (radioGroupId);
						prop.SetValue (obj, radioGroup);
						break;	
					case "RadioButton":
						var radioButtonId = ControlUtil.GetResourceId (pn);
						var radioButton = obj.FindViewById<RadioButton> (radioButtonId);
						prop.SetValue (obj, radioButton);
						break;	
					case "CheckBox":
						var checkBoxId = ControlUtil.GetResourceId (pn);
						var checkBox = obj.FindViewById<CheckBox> (checkBoxId);
						prop.SetValue (obj, checkBox);
						break;	
					}
				}
			}
		}

		public static void CreateControls<T> (this T obj, View view)
		{
			if (obj != null) {
				foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
					var pn = prop.Name;
					var pt = prop.PropertyType;
					switch (pt.Name) {
					case "Button":
						var buttonId = ControlUtil.GetResourceId (pn);
						var button = view.FindViewById<Button> (buttonId);
						prop.SetValue (obj, button);
						break;
					case "ListView":
						var listViewId = ControlUtil.GetResourceId (pn);
						var listView = view.FindViewById<ListView> (listViewId);
						prop.SetValue (obj, listView);
						break;
					case "ScrollView":
						var scrollViewId = ControlUtil.GetResourceId (pn);
						var scrollView = view.FindViewById<ScrollView> (scrollViewId);
						prop.SetValue (obj, scrollView);
						break;
					case "LinearLayout":
						var linearLayoutId = ControlUtil.GetResourceId (pn);
						var linearLayout = view.FindViewById<LinearLayout> (linearLayoutId);
						prop.SetValue (obj, linearLayout);
						break;
					case "RelativeLayout":
						var relativeLayoutId = ControlUtil.GetResourceId (pn);
						var relativeLayout = view.FindViewById<RelativeLayout> (relativeLayoutId);
						prop.SetValue (obj, relativeLayout);
						break;
					case "TextView":
						var textViewId = ControlUtil.GetResourceId (pn);
						var textView = view.FindViewById<TextView> (textViewId);
						prop.SetValue (obj, textView);
						break;
					case "EditText":
						var editTextId = ControlUtil.GetResourceId (pn);
						var editText = view.FindViewById<EditText> (editTextId);
						prop.SetValue (obj, editText);
						break;
					case "Spinner":
						var spinnerId = ControlUtil.GetResourceId (pn);
						var spinner = view.FindViewById<Spinner> (spinnerId);
						prop.SetValue (obj, spinner);
						break;
					case "ImageView":
						var imageViewId = ControlUtil.GetResourceId (pn);
						var imageView = view.FindViewById<ImageView> (imageViewId);
						prop.SetValue (obj, imageView);
						break;	
					case "ImageButton":
						var imageButtonId = ControlUtil.GetResourceId (pn);
						var imageButton = view.FindViewById<ImageButton> (imageButtonId);
						prop.SetValue (obj, imageButton);
						break;	
					case "RadioGroup":
						var radioGroupId = ControlUtil.GetResourceId (pn);
						var radioGroup = view.FindViewById<RadioGroup> (radioGroupId);
						prop.SetValue (obj, radioGroup);
						break;	
					case "RadioButton":
						var radioButtonId = ControlUtil.GetResourceId (pn);
						var radioButton = view.FindViewById<RadioButton> (radioButtonId);
						prop.SetValue (obj, radioButton);
						break;	
					}
				}
			}
		}
	}

}
