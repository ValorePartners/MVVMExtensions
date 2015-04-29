using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Foundation;
using UIKit;

namespace MobileRef.MVVM.IOS
{
	public class LinkerPrepare
	{
		public static bool AlreadyPrepared{ get; set; }

		public static nint TagIndex = 0;

		public static void Init ()
		{
			if (!AlreadyPrepared) {
				UIButton button = new UIButton ();
				button.TouchUpInside += PrepHandler;
				button.TouchUpInside -= PrepHandler;
				button.Tag = 0;
				var tag = button.Tag;

				UIBarButtonItem barButton = new UIBarButtonItem (UIBarButtonSystemItem.Stop);
				barButton.Clicked += PrepHandler;
				barButton.Clicked -= PrepHandler;
				barButton.Tag = 0;
				tag = barButton.Tag;

				UILabel label = new UILabel ();
				label.Tag = 0;
				label.Text = "";
				var x = label.Text;
				tag = label.Tag;

				UITextView utv = new UITextView ();
				utv.Tag = 0;
				utv.Text = "";
				x = utv.Text;
				tag = utv.Tag;

				UITextField uiT = new UITextField ();
				uiT.EditingChanged += PrepHandler;
				uiT.EditingChanged -= PrepHandler;
				uiT.Tag = 0;
				uiT.Text = "";
				x = uiT.Text;
				tag = uiT.Tag;

				UISwitch us = new UISwitch ();
				us.ValueChanged += PrepHandler;
				us.ValueChanged -= PrepHandler;
				us.Tag = 0;

				AlreadyPrepared = true;
			}

		}

		private static void PrepHandler (object sender, EventArgs args)
		{

		}
	}

	[Preserve (AllMembers = true)]
	public class PropertyBindingSettings
	{
		public nint Tag {
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
		public nint Tag {
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
		where T: UIViewController, IHandlers
		where K:INotifyPropertyChanged
	{
		private List<PropertyBindingSettings> pbs;
		private List<EventBindingSettings> ebs;
		private K viewModel;
		private T controller;

		public void BindProperty (string bindingObject, string bindingProperty, string eventName, string viewModelProperty, string format)
		{
			pbs.Add (new PropertyBindingSettings () {
				BindingObject = bindingObject,
				BindingProperty = bindingProperty,
				ViewModelProperty = viewModelProperty,
				EventName = eventName,
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
			var controlName = GetControlName (bindingProperty);
			var bpFieldName = GetPropertyName (bindingProperty);
			var bpName = GetPropertyTypeName (bindingProperty);
			switch (bpName) {
			case "UITextField":	
				eventName = "EditingChanged";
				break;
			case "UISwitch":
				eventName = "ValueChanged";
				break;
			case "UITableViewSource":
				if (bpFieldName == "SelectedItem")
					eventName = "RowSelectedEvent";
				break;
			}

			var vmFieldName = GetPropertyName (viewModelProperty);

			BindProperty (controlName, bpFieldName, eventName, vmFieldName, format);

		}

		public void BindCommand (string bindingObject, string eventName, string viewModelCommand)
		{
			ebs.Add (new EventBindingSettings () {
				BindingObject = bindingObject,
				EventName = eventName,
				ViewModelCommand = viewModelCommand,
				Tag = LinkerPrepare.TagIndex++
			});
		}

		public void BindCommand (Expression<Func<object>> bindingProperty, Expression<Func<object>> viewModelProperty)
		{
			string eventName = null;
			var controlName = GetCommandControlName (bindingProperty);
			var bpName = GetCommandTypeName (bindingProperty);
			var vmFieldName = GetPropertyName (viewModelProperty);
			switch (bpName) {
			case "UIButton":	
				eventName = "TouchUpInside";
				break;
			case "UIBarButtonItem":
				eventName = "Clicked";
				break;
			}
			BindCommand (controlName, eventName, vmFieldName);
		}

		public void RegisterBindingEvents (T controller, K viewModel, bool registerDefaultControls = true)
		{
			LinkerPrepare.Init ();
			this.controller = controller;
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
				controller.RegisterEvents (this.ebs);
			}

		}

		public void UnRegisterBindingEvents (T controller, bool unRegisterDefaultControls = true)
		{
			foreach (var obj in pbs) {
				DisconnectPropertyBindingSetting (obj);
			}
			foreach (var obj in ebs) {
				DisconnectCommandBindingSetting (obj);
			}

			if (unRegisterDefaultControls) {
				controller.UnRegisterEvents ();
			}

		}

		public BindingManager ()
		{
			pbs = new List<PropertyBindingSettings> ();
			ebs = new List<EventBindingSettings> ();
		}

		private void InternalControlEventHandler (object sender, EventArgs args)
		{
			nint tag = 0;
			if (sender is UIControl) {
				tag = ((UIControl)sender).Tag;
			} else {
				if (sender is IBaseSource)
					tag = ((IBaseSource)sender).Tag;
			}

			PropertyBindingSettings ctrl = null;
			if (args != null && args is RowSelectedEventArgs) {
				ctrl = pbs.FirstOrDefault (x => x.Tag == tag && x.EventName == "RowSelectedEvent");
			} else {
				ctrl = pbs.FirstOrDefault (x => x.Tag == tag);
			}
				
			if (ctrl != null) {
				var prop = sender.GetType ().GetProperty (ctrl.BindingProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
				var propValue = prop.GetValue (sender, null);
				var vmProp = viewModel.GetType ().GetProperty (ctrl.ViewModelProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

				if (sender is IBaseSource) {

						var item = ((IBaseSource)sender).SelectedItem;
						vmProp.SetValue (viewModel, item);
		
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
			nint tag = 0;
			if(sender is UIControl)
				tag = ((UIControl)sender).Tag;
			if(sender is IBaseSource)
				tag = ((IBaseSource)sender).Tag;
			var ctrl = ebs.FirstOrDefault (x => x.Tag == tag);

			if (ctrl != null) {
				var vmProp = typeof(K).GetProperty (ctrl.ViewModelCommand, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
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
								var ib = ctrlProp.PropertyType.GetInterface ("IBaseSource");
								if (ib != null) {
									if (obj.EventName != "RowSelectedEvent") {
										var ctrlInstance = ctrlProp.GetValue (controller);
										((IBaseSource)ctrlInstance).UpdateCollection ((ICollection)sender);
									}
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
					var ctrlInstance = ctrlProp.GetValue (controller);
					var ctrlInstaceProp = ctrlInstance.GetType ().GetProperty (obj.BindingProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var currentValue = ctrlInstaceProp.GetValue (ctrlInstance);
					var vmProp = sender.GetType ().GetProperty (obj.ViewModelProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var vmValue = vmProp.GetValue (sender, null);

					if (ctrlInstance is IBaseSource) {
						if (obj.EventName != "RowSelectedEvent") {
							((IBaseSource)ctrlInstance).UpdateCollection ((ICollection)vmValue);
						}

					} else if (currentValue.GetType ().Name != "String" && (currentValue.GetType ().Name == vmValue.GetType ().Name)) {
						ctrlInstaceProp.SetValue (ctrlInstance, vmValue);
					} else {
						if (!currentValue.Equals (vmValue)) {
							if (!string.IsNullOrEmpty (obj.Format)) {
								var formattedValue = string.Format (obj.Format, vmValue);
								ctrlInstaceProp.SetValue (ctrlInstance, formattedValue);
							} else {
								ctrlInstaceProp.SetValue (ctrlInstance, vmValue.ToString ());
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
			var prop = typeof(T).GetProperty (setting.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (controller);

					var tagProp = instance.GetType ().GetProperty ("Tag", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					if (tagProp != null)
						tagProp.SetValue (instance, setting.Tag);

					var eventInstance = instance.GetType ().GetEvent (setting.EventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
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
			var prop = typeof(T).GetProperty (setting.BindingObject, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (prop != null) {
				if (!string.IsNullOrEmpty (setting.EventName)) {
					var instance = prop.GetValue (controller);

					var tagProp = instance.GetType ().GetProperty ("Tag", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					tagProp.SetValue (instance, setting.Tag);

					var eventInstance = instance.GetType ().GetEvent (setting.EventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
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
					var instance = prop.GetValue (controller);
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
					var instance = prop.GetValue (controller);
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

		private string GetPropertyTypeName (Expression<Func<object>> propertyRefExpr)
		{
			if (propertyRefExpr.Body is MemberExpression) {
				var exp = ((MemberExpression)propertyRefExpr.Body);
				var tableSource = ((PropertyInfo)exp.Member).DeclaringType.BaseType.Name;
				if (tableSource == "UITableViewSource")
					return "UITableViewSource";
				return ((PropertyInfo)exp.Member).DeclaringType.Name;

			} else {
				var unaryExpr = propertyRefExpr.Body as UnaryExpression;
				var memberExpr = unaryExpr.Operand as MemberExpression;
				return ((PropertyInfo)memberExpr.Member).DeclaringType.Name;
			}
		}

		private string GetCommandTypeName (Expression<Func<object>> propertyRefExpr)
		{
			var exp = ((MemberExpression)propertyRefExpr.Body);
			return ((PropertyInfo)exp.Member).PropertyType.Name;
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

	}

	public static class FrameworkExtensions
	{
		public static void RegisterEvents<T> (this T obj) 
			where T: UIViewController, IHandlers
		{
			obj.RegisterEvents (new List<EventBindingSettings> ());
		}

		public static void RegisterEvents<T> (this T obj, List<EventBindingSettings> ebs) 
			where T: UIViewController, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;
				if (ControlUtil.ControlList.Contains (pt.Name)) {
					var ctrl = prop.GetValue (obj);
					if(ctrl is UIControl){
						((UIControl)ctrl).AccessibilityIdentifier = prop.Name; //allows for TestCloud integration
						((UIControl)ctrl).AccessibilityLabel = prop.Name;
					}
					if (!ebs.Any (x => x.BindingObject == pn)) {
						ControlUtil.HookUIControl (ctrl, obj);
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
					foreach (var vmProp in vm.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
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
			where T: UIViewController, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;
				if (ControlUtil.ControlList.Contains (pt.Name)) {
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
	}
}

