using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Linq.Expressions;
using System.Collections;
using MobileRef.MVVM.Shared;
using System.Threading;
using Java.Net;


namespace MobileRef.MVVM.Android
{
	public static class BaseActivityExtensions
	{
		public static void ExitApplication (this Activity sender)
		{
			System.Environment.Exit (0);
		}

		public static void ShowMessage (this Activity sender, string title, string msg)
		{
			try {
				var dlg = new AlertDialog.Builder (sender).Create ();
				dlg.SetTitle (title);
				dlg.SetMessage (msg);
				dlg.SetCancelable (false);
				dlg.SetButton ("Okay", ( cs, ce) => {

				});
				dlg.Show ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}


		public static void ShowMessage (this Activity sender, string title, string msg, string button1Text, Action button1)
		{
			try {
				var dlg = new AlertDialog.Builder (sender).Create ();
				dlg.SetTitle (title);
				dlg.SetMessage (msg);
				dlg.SetCancelable (false);
				dlg.SetButton (button1Text, ( cs, ce) => {
					button1.Invoke ();
				});
				dlg.Show ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}

		public static void ShowMessage (this Activity sender, string title, string msg, string button1Text, Action button1, string button2Text, Action button2)
		{
			try {
				var dlg = new AlertDialog.Builder (sender).Create ();
				dlg.SetTitle (title);
				dlg.SetMessage (msg);
				dlg.SetCancelable (false);
				dlg.SetButton (button1Text, ( cs, ce) => {
					button1.Invoke ();
				});
				dlg.SetButton2 (button2Text, ( cs, ce) => {
					button2.Invoke ();
				});
				dlg.Show ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}

		public static void ShowMessage (this Activity sender, string title, string msg, string button1Text, Action button1, string button2Text, Action button2, string button3Text, Action button3)
		{
			try {
				var dlg = new AlertDialog.Builder (sender).Create ();
				dlg.SetTitle (title);
				dlg.SetMessage (msg);
				dlg.SetCancelable (false);
				dlg.SetButton (button1Text, ( cs, ce) => {
					button1.Invoke ();
				});
				dlg.SetButton2 (button2Text, ( cs, ce) => {
					button2.Invoke ();
				});
				dlg.SetButton3 (button3Text, ( cs, ce) => {
					button3.Invoke ();
				});
				dlg.Show ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}

		public static void ShowMessage (this Activity sender, string title, string buttonText, Action buttonAction, View view)
		{
			var objDialog = new AlertDialog.Builder (sender);
			objDialog.SetTitle (title);
			objDialog.SetView (view);
			objDialog.SetPositiveButton (buttonText, (oe, oa) => {
				buttonAction.Invoke ();
			});
			objDialog.SetNegativeButton ("Cancel", (oec, oac) => {
			});
			objDialog.Show ();
		}

		public static void PlaySoundFile (this Activity sender, int resourceId)
		{
			using (var player = MediaPlayer.Create (Application.Context, resourceId)) {
				player.Start ();
			}
		}

		public static void NavigateActivity<T> (this Activity context, bool finish = false)where T : Activity
		{
			var intent = new Intent (context, typeof(T));
			context.StartActivity (intent);
			if (finish)
				context.Finish ();

		}

		public static void NavigateActivity<T> (this Activity context, Action<T> action, bool finish = false) where T : Activity
		{
			var intent = new Intent (context, typeof(T));
			ActivityBridge.FinishedLoading = pageLoaded => {
				action.Invoke ((T)pageLoaded);
			};
			context.StartActivity (intent);
			if (finish)
				context.Finish ();
		}

		public static void NavigateActivity<T> (this Activity context, string propName, object objValue, bool finish = false) where T : Activity
		{
			var intent = new Intent (context, typeof(T));
			ActivityBridge.FinishedLoading = pageLoaded => {
				var obj = (T)pageLoaded;
				var prop = typeof(T).GetProperty (propName);
				if (prop != null)
					prop.SetValue (obj, objValue);
			};
			context.StartActivity (intent);
			if (finish)
				context.Finish ();
		}

		public static void NavigateActivityByExtra<T> (this Activity context, string putName, string putValue, bool finish = false) where T : Activity
		{
			var intent = new Intent (context, typeof(T));
			intent.PutExtra (putName, putValue);
			context.StartActivity (intent);
			if (finish)
				context.Finish ();
		}

		public static void CreateBridge (this Activity context)
		{
			if (ActivityBridge.FinishedLoading != null) {
				ActivityBridge.FinishedLoading (context);
				ActivityBridge.FinishedLoading = null;
			}
		}

		public static void SetDialogTitleDivider (this Activity dialog, string hexColor)
		{
			var decorView = (ViewGroup)dialog.Window.DecorView;
			var windowContentView = ViewGroupIndexOf<ViewGroup> (decorView, 0);
			var titleDivider = ViewGroupIndexOf<View> (windowContentView, 1);
			titleDivider.SetBackgroundColor (global::Android.Graphics.Color.ParseColor (hexColor));
		}

		private static T ViewGroupIndexOf<T> (ViewGroup grp, int index) where T : View
		{
			return (T)grp.GetChildAt (index);
		}

		public static void PerformEmail (this Activity context, string emailAddress, string subject, string message, string editorTitle)
		{
			var emailIntent = new Intent (Intent.ActionSend);
			emailIntent.SetType ("message/rfc822");
			emailIntent.PutExtra (Intent.ExtraEmail, new string[] { emailAddress });
			emailIntent.PutExtra (Intent.ExtraSubject, subject);
			emailIntent.PutExtra (Intent.ExtraText, message);
			context.StartActivity (Intent.CreateChooser (emailIntent, editorTitle));
		}

		public static void PerformEmail (this Activity context, string emailAddress, string subject, string editorTitle)
		{
			var emailIntent = new Intent (Intent.ActionSend);
			emailIntent.SetType ("message/rfc822");
			emailIntent.PutExtra (Intent.ExtraEmail, new string[] { emailAddress });
			emailIntent.PutExtra (Intent.ExtraSubject, subject);
			context.StartActivity (Intent.CreateChooser (emailIntent, editorTitle));
		}

		public static void PerformCall (this Activity context, string phoneNumber)
		{
			var phNum = string.Format ("tel:{0}", phoneNumber);
			var uri = global::Android.Net.Uri.Parse (phNum);
			var intent = new Intent (Intent.ActionView, uri);
			context.StartActivity (intent); 
		}

		public static void PerformSMS (this Activity context, string phoneNumber)
		{
			var smsAddress = string.Format ("smsto:{0}", phoneNumber);
			var smsUri = global::Android.Net.Uri.Parse (smsAddress);
			var smsIntent = new Intent (Intent.ActionSendto, smsUri);
			context.StartActivity (smsIntent);
		}

		public static void PerformMap (this Activity context, string address, string city, string state, string zip)
		{
			var geoAddress = string.Format ("geo:0,0?q={0}", address
			                 + " " + city
			                 + " " + state
			                 + " " + zip);

			var geoUri = global::Android.Net.Uri.Parse (geoAddress);
			var mapIntent = new Intent (Intent.ActionView, geoUri);
			context.StartActivity (mapIntent);
		}

		public static void PerformMap (this Activity context, long latitute, long longitude)
		{
			var intent = new Intent (Intent.ActionView, global::Android.Net.Uri.Parse ("geo:" + latitute + "," + longitude));
			context.StartActivity (intent);
		}
	}


	[Preserve (AllMembers = true)]
	public static class BaseFragmentExtensions
	{

		public static void NavigateActivity<T> (this Fragment context, bool finish = false)where T : Activity
		{
			context.Activity.NavigateActivity<T> (finish);
		}

		public static void NavigateActivity<T> (this Fragment context, string propName, object objValue, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivity<T> (propName, objValue, finish);
		}

		public static void NavigateActivity<T> (this Fragment context, Action<T> action, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivity<T> (action, finish);
		}

		public static void NavigateActivityByExtra<T> (this Fragment context, string putName, string putValue, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivityByExtra<T> (putName, putValue, finish);
		}
			

	}

	[Preserve (AllMembers = true)]
	public static class ControlExtensions
	{
		public static void LoadUrl (this ImageView img, Activity activity, string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient ()) {
				var imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}
			img.SetImageBitmap (imageBitmap);
			Runnable run = new Runnable (img.Invalidate);
			activity.RunOnUiThread (run);
		}
		public static void LoadUrlBySynchronizationContext (this ImageView img, string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient ()) {
				var imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}
			img.SetImageBitmap (imageBitmap);
			Application.SynchronizationContext.Post (_ => {
				img.SetImageBitmap (imageBitmap);
				img.Invalidate();
			}, null);
		}

		public static void ResetHeight (this ListView listView)
		{
			if (listView == null || listView.Adapter == null) {
				return;
			}
			try {
				var row = 0;
				int totalHeight = listView.PaddingTop + listView.PaddingBottom;
				for (int i = 0; i < listView.Count; i++) {
					View listItem = listView.Adapter.GetView (i, null, listView);
					var p = listItem.LayoutParameters;
					var v = ((ViewGroup)listItem);
					if (listItem.GetType () == typeof(ViewGroup)) {
						listItem.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
					}
					listItem.Measure (0, 0);
					row = listItem.MeasuredHeight;
				}

				listView.LayoutParameters.Height = (row * listView.Count) + (listView.DividerHeight * (listView.Count - 1));
				listView.RequestLayout ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}
	}



	[Preserve (AllMembers = true)]
	public static class ActivityBridge
	{
		public static Action<object> FinishedLoading { get; set; }
	}

	[Preserve (AllMembers = true)]
	[System.AttributeUsage (System.AttributeTargets.Property)]
	public class RegisterEvent : System.Attribute
	{
		public string[] EventNames{ get; set; }

		public RegisterEvent (string eventName)
		{
			EventNames = new string[]{ eventName };
		}

		public RegisterEvent (string[] eventNames)
		{
			EventNames = eventNames;
		}
	}

	[Preserve (AllMembers = true)]
	public abstract class ViewHolder<T>:Java.Lang.Object
		where T:class
	{
		public abstract int CellResourceId{ get; }

		public View CreateView (LayoutInflater inflator)
		{
			return inflator.Inflate (CellResourceId, null);
		}

		public abstract void Init (View view);

		public abstract void Populate (T item);
	}

	public interface IBaseAdapter
	{
		void UpdateCollection (ICollection collection);

	}

	[Preserve (AllMembers = true)]
	public class HolderAdapter<T,K>:BaseAdapter<T>,IBaseAdapter
		where T: class
		where K : ViewHolder<T>, new()
	{
		public LayoutInflater Inflator { get; set; }

		public T[] Items{ get; set; }

		public HolderAdapter (LayoutInflater inflator, T[] items)
		{
			this.Inflator = inflator;
			this.Items = items;
		}

		#region IBaseAdapter implementation

		public void UpdateCollection (ICollection collection)
		{
			var List = new List<T> ();
			foreach (var item in collection) {
				List.Add ((T)item);
			}
			Items = List.ToArray ();
			this.NotifyDataSetChanged ();
		}

		#endregion

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			K holder;
			var item = Items [position];
			var view = convertView;
			if (view == null) {
				holder = new K ();
				view = holder.CreateView (Inflator);
				holder.Init (view);
				view.Tag = holder;
			} else {
				holder = view.Tag as K;
			}
			holder.Populate (item);
			return view;
		}

		public override int Count {
			get {
				return Items.Count ();
			}
		}

		public override T this [int index] {
			get {
				return Items [index];
			}
		}

	

	}

	[Preserve (AllMembers = true)]
	public class Ctrl
	{
		//public static string[] ControlList = { "Button", "ListView", "Spinner", "ImageButton", "RadioGroup" };

		public static StringDictionary ActionDictionary = new StringDictionary () {
			{ "Button", "Click" },
			{ "ListView", "ItemClick" },
			{ "Spinner", "ItemSelected" },
			{ "ImageButton", "Click" },
			{ "RadioGroup", "CheckedChange" },
		};

		public static void HookUIControl (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				if (ActionDictionary.ContainsKey (contrlType)) {
					var eventName = ActionDictionary [contrlType];
					var eventInstance = ctrl.GetType ().GetEvent (eventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = handler.GetType ().GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetAddMethod ();
							var d = Delegate.CreateDelegate (tDelegate, handler, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (ctrl, addHandlerArgs);
						}
					}
				}
			}

		}

		public static void UnHookUIControll (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				if (ActionDictionary.ContainsKey (contrlType)) {
					var eventName = ActionDictionary [contrlType];
					var eventInstance = ctrl.GetType ().GetEvent (eventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = handler.GetType ().GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetRemoveMethod ();
							var d = Delegate.CreateDelegate (tDelegate, handler, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (ctrl, addHandlerArgs);
						}
					}
				}
			}

		}

		public static int GetResourceId (string controlName)
		{
			var rType = typeof(Resource.Id); 
			FieldInfo field = null;
			foreach (var obj in rType.GetFields()) {
				if (obj.Name.Equals (controlName)) {
					field = obj;
					return (int)field.GetValue (controlName);
				}
			}
			return 0;
		}

	}

	[Preserve (AllMembers = true)]
	public interface IHandlers
	{
		void ViewEventHandler (object e, EventArgs args);

		void ControlsHandler (object sender, EventArgs args);
	}

	public static class TConverter
	{
		public static T ChangeType<T> (object value)
		{
			return (T)ChangeType (typeof(T), value);
		}

		public static object ChangeType (Type t, object value)
		{
			try {
				var tc = TypeDescriptor.GetConverter (t);
				return tc.ConvertFrom (value);
			} catch {
				return null;
			}
		}

		public static void RegisterTypeConverter<T, TC> () where TC : TypeConverter
		{

			TypeDescriptor.AddAttributes (typeof(T), new TypeConverterAttribute (typeof(TC)));
		}
	}

	public class ValueConverter
	{
		public static object ChangeType (string typeName, string stringValue)
		{
			object obj = null;
			switch (typeName) {
			case "Int32":
				int intValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!int.TryParse (stringValue, out intValue))
						obj = null;
				} else {
					intValue = 0;
				}
				obj = intValue;
				break;
			case "Boolean":
				bool boolValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!bool.TryParse (stringValue, out boolValue))
						return null;
				} else {
					boolValue = false;
				}
				obj = boolValue;
				break;
			case "Single":
				float floatValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!float.TryParse (stringValue, out floatValue))
						obj = null;
				} else {
					floatValue = 0.0f;
				}
				obj = floatValue;
				break;
			case "Double":
				double doubleValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!double.TryParse (stringValue, out doubleValue))
						obj = null;
				} else {
					doubleValue = 0.0d;
				}
				obj = doubleValue;
				break;
			case "DateTime":
				DateTime dtValue = DateTime.Now;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!DateTime.TryParse (stringValue, out dtValue))
						obj = null;
					else {
						obj = dtValue;
					}
				} else {
					obj = null;
				}
				break;
			default:
				obj = stringValue;
				break;
			}
			return obj;	
		}
	}

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
		where T: Activity, IHandlers
		where K:INotifyPropertyChanged
	{
		private List<PropertyBindingSettings> pbs;
		private List<EventBindingSettings> ebs;
		private K viewModel;
		private T activity;

		public void BindProperty (string bindingObject, string bindingProperty, string eventName, string viewModelProperty, string format)
		{
			pbs.Add (new PropertyBindingSettings () {
				BindingObject = bindingObject,
				BindingProperty = bindingProperty,
				ViewModelProperty = viewModelProperty,
				EventName = eventName,
				ResourceId = Ctrl.GetResourceId (bindingObject),
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
				ResourceId = Ctrl.GetResourceId (bindingObject)
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

		public void RegisterBindingEvents (T activity, K viewModel, bool registerDefaultControls = true)
		{
			LinkerPrepare.Init (activity);
			this.activity = activity;
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
				activity.RegisterEvents (this.ebs);
			}

		}

		public void UnRegisterBindingEvents (T activity,bool unRegisterDefaultControls = true)
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
				activity.UnRegisterEvents ();
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
									var ctrlInstance = ctrlProp.GetValue (activity);
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
					var ctrlInstance = ctrlProp.GetValue (activity);
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
					var instance = prop.GetValue (activity);
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
					var instance = prop.GetValue (activity);
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
					var instance = prop.GetValue (activity);
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
					var instance = prop.GetValue (activity);
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
			where T: Activity, IHandlers
		{
			obj.RegisterEvents (new List<EventBindingSettings> ());
		}

		public static void RegisterEvents<T> (this T obj, List<EventBindingSettings> ebs) 
			where T: Activity, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;

				if (Ctrl.ActionDictionary.ContainsKey (pt.Name)) {
					var eventName = Ctrl.ActionDictionary [pt.Name];
					if (!ebs.Any (x => x.BindingObject == prop.Name && x.EventName == eventName)) {
						Ctrl.HookUIControl (prop.GetValue (obj), obj);
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
			where T: Activity, IHandlers
		{
			var iEvent = (IHandlers)obj;
			foreach (var prop in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
				var pn = prop.Name;
				var pt = prop.PropertyType;

				if (Ctrl.ActionDictionary.ContainsKey (pt.Name)) {
					Ctrl.UnHookUIControll (prop.GetValue (obj), obj);
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
						var buttonId = Ctrl.GetResourceId (pn);
						var button = obj.FindViewById<Button> (buttonId);
						prop.SetValue (obj, button);
						break;
					case "ListView":
						var listViewId = Ctrl.GetResourceId (pn);
						var listView = obj.FindViewById<ListView> (listViewId);
						prop.SetValue (obj, listView);
						break;
					case "ScrollView":
						var scrollViewId = Ctrl.GetResourceId (pn);
						var scrollView = obj.FindViewById<ScrollView> (scrollViewId);
						prop.SetValue (obj, scrollView);
						break;
					case "LinearLayout":
						var linearLayoutId = Ctrl.GetResourceId (pn);
						var linearLayout = obj.FindViewById<LinearLayout> (linearLayoutId);
						prop.SetValue (obj, linearLayout);
						break;
					case "RelativeLayout":
						var relativeLayoutId = Ctrl.GetResourceId (pn);
						var relativeLayout = obj.FindViewById<RelativeLayout> (relativeLayoutId);
						prop.SetValue (obj, relativeLayout);
						break;
					case "TextView":
						var textViewId = Ctrl.GetResourceId (pn);
						var textView = obj.FindViewById<TextView> (textViewId);
						prop.SetValue (obj, textView);
						break;
					case "EditText":
						var editTextId = Ctrl.GetResourceId (pn);
						var editText = obj.FindViewById<EditText> (editTextId);
						prop.SetValue (obj, editText);
						break;
					case "Spinner":
						var spinnerId = Ctrl.GetResourceId (pn);
						var spinner = obj.FindViewById<Spinner> (spinnerId);
						prop.SetValue (obj, spinner);
						break;
					case "ImageView":
						var imageViewId = Ctrl.GetResourceId (pn);
						var imageView = obj.FindViewById<ImageView> (imageViewId);
						prop.SetValue (obj, imageView);
						break;	
					case "ImageButton":
						var imageButtonId = Ctrl.GetResourceId (pn);
						var imageButton = obj.FindViewById<ImageButton> (imageButtonId);
						prop.SetValue (obj, imageButton);
						break;	
					case "RadioGroup":
						var radioGroupId = Ctrl.GetResourceId (pn);
						var radioGroup = obj.FindViewById<RadioGroup> (radioGroupId);
						prop.SetValue (obj, radioGroup);
						break;	
					case "RadioButton":
						var radioButtonId = Ctrl.GetResourceId (pn);
						var radioButton = obj.FindViewById<RadioButton> (radioButtonId);
						prop.SetValue (obj, radioButton);
						break;	
					case "CheckBox":
						var checkBoxId = Ctrl.GetResourceId (pn);
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
						var buttonId = Ctrl.GetResourceId (pn);
						var button = view.FindViewById<Button> (buttonId);
						prop.SetValue (obj, button);
						break;
					case "ListView":
						var listViewId = Ctrl.GetResourceId (pn);
						var listView = view.FindViewById<ListView> (listViewId);
						prop.SetValue (obj, listView);
						break;
					case "ScrollView":
						var scrollViewId = Ctrl.GetResourceId (pn);
						var scrollView = view.FindViewById<ScrollView> (scrollViewId);
						prop.SetValue (obj, scrollView);
						break;
					case "LinearLayout":
						var linearLayoutId = Ctrl.GetResourceId (pn);
						var linearLayout = view.FindViewById<LinearLayout> (linearLayoutId);
						prop.SetValue (obj, linearLayout);
						break;
					case "RelativeLayout":
						var relativeLayoutId = Ctrl.GetResourceId (pn);
						var relativeLayout = view.FindViewById<RelativeLayout> (relativeLayoutId);
						prop.SetValue (obj, relativeLayout);
						break;
					case "TextView":
						var textViewId = Ctrl.GetResourceId (pn);
						var textView = view.FindViewById<TextView> (textViewId);
						prop.SetValue (obj, textView);
						break;
					case "EditText":
						var editTextId = Ctrl.GetResourceId (pn);
						var editText = view.FindViewById<EditText> (editTextId);
						prop.SetValue (obj, editText);
						break;
					case "Spinner":
						var spinnerId = Ctrl.GetResourceId (pn);
						var spinner = view.FindViewById<Spinner> (spinnerId);
						prop.SetValue (obj, spinner);
						break;
					case "ImageView":
						var imageViewId = Ctrl.GetResourceId (pn);
						var imageView = view.FindViewById<ImageView> (imageViewId);
						prop.SetValue (obj, imageView);
						break;	
					case "ImageButton":
						var imageButtonId = Ctrl.GetResourceId (pn);
						var imageButton = view.FindViewById<ImageButton> (imageButtonId);
						prop.SetValue (obj, imageButton);
						break;	
					case "RadioGroup":
						var radioGroupId = Ctrl.GetResourceId (pn);
						var radioGroup = view.FindViewById<RadioGroup> (radioGroupId);
						prop.SetValue (obj, radioGroup);
						break;	
					case "RadioButton":
						var radioButtonId = Ctrl.GetResourceId (pn);
						var radioButton = view.FindViewById<RadioButton> (radioButtonId);
						prop.SetValue (obj, radioButton);
						break;	
					}
				}
			}
		}
	}

	// THIS IS THE REACHABILITY PORTION OF THE CODE
	public enum NetworkState
	{
		Unknown,
		ConnectedWifi,
		ConnectedData,
		Disconnected
	}

	public class NetworkStatusMonitor
	{
		private static NetworkState _state;

		public NetworkStatusMonitor ()
		{
		}

		public static NetworkState State {
			get { 
				UpdateNetworkStatus ();

				return _state; 
			}
		}

		public static void UpdateNetworkStatus ()
		{
			_state = NetworkState.Unknown;

			// Retrieve the connectivity manager service
			var connectivityManager = (ConnectivityManager)
				Application.Context.GetSystemService (
				                          Context.ConnectivityService);

			// Check if the network is connected or connecting.
			// This means that it will be available, 
			// or become available in a few seconds.
			var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;

			if (activeNetworkInfo != null) {
				if (activeNetworkInfo.IsConnectedOrConnecting) {
					// Now that we know it's connected, determine if we're on WiFi or something else.
					_state = activeNetworkInfo.Type == ConnectivityType.Wifi ?
						NetworkState.ConnectedWifi : NetworkState.ConnectedData;
				} else {
					_state = NetworkState.Disconnected;
				}
			} else {
				_state = NetworkState.Disconnected;
			}
		}
	}

	[BroadcastReceiver ()]
	public class NetworkStatusBroadcastReceiver: BroadcastReceiver
	{

		public event EventHandler ConnectionStatusChanged;

		public override void OnReceive (Context context, Intent intent)
		{
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged (this, EventArgs.Empty);
		}
	}

	public class Reachability
	{
		private static NetworkStatusBroadcastReceiver m_BroadcastReceiver;

		public static NetworkState CurrentConnectionStatus { get; set; }

		private static void OnNetworkStatusChanged (object sender, EventArgs e)
		{
			var state = NetworkStatusMonitor.State;
			if (state != CurrentConnectionStatus) {
				//Status has changed
				CurrentConnectionStatus = state;
				#if DEBUG
				Console.WriteLine ("******* Status Event Fired " + state + " ************");
				#endif
				if (CurrentConnectionStatus == NetworkState.Disconnected || CurrentConnectionStatus == NetworkState.Unknown) {
					AppData.IsConnected = false;
					//MessageHub.ConnectionStatusChangedEvent ();
				} else {
					AppData.IsConnected = true;
					//MessageHub.ConnectionStatusChangedEvent ();
				}
			}

		}

		public static void Start ()
		{
			if (m_BroadcastReceiver != null) {
				throw new InvalidOperationException (
					"Network status monitoring already active.");
			}
			// Create the broadcast receiver and bind the event handler
			// so that the app gets updates of the network connectivity status
			m_BroadcastReceiver = new NetworkStatusBroadcastReceiver ();
			m_BroadcastReceiver.ConnectionStatusChanged += OnNetworkStatusChanged;
			CurrentConnectionStatus = NetworkStatusMonitor.State;
			if (CurrentConnectionStatus == NetworkState.Disconnected || CurrentConnectionStatus == NetworkState.Unknown) {
				AppData.IsConnected = false;
				//MessageHub.ConnectionStatusChangedEvent ();
			} else {
				AppData.IsConnected = true;
				//MessageHub.ConnectionStatusChangedEvent ();
			}
			// Register the broadcast receiver
			Application.Context.RegisterReceiver (m_BroadcastReceiver, new IntentFilter (ConnectivityManager.ConnectivityAction));
		}

		public static void Stop ()
		{
			if (m_BroadcastReceiver == null) {
				throw new InvalidOperationException (
					"Network status monitoring not active.");
			}

			// Unregister the receiver so we no longer get updates.
			Application.Context.UnregisterReceiver (m_BroadcastReceiver);

			// Set the variable to nil, so that we know the receiver is no longer used.
			m_BroadcastReceiver.ConnectionStatusChanged -= OnNetworkStatusChanged;
			m_BroadcastReceiver = null;

		}
	}

	public class ImageDownloader
	{
		private static readonly Dictionary<string, Bitmap> UrlToImageMap;

		static ImageDownloader()
		{
			UrlToImageMap = new Dictionary<string, Bitmap>();
		}

		public static void AssignImageAsync(ImageView image, string url)
		{
			var info = new ImageDownloadInfo
			{
				ImageView = image,
				ImageUrl = url,
			};

			ThreadPool.QueueUserWorkItem(DownloadImage, info);
		}

		private static void DownloadImage(object state)
		{
			try
			{
				var info = (ImageDownloadInfo)state;
				Bitmap bitmap;
				lock (UrlToImageMap)
				{
					if (UrlToImageMap.ContainsKey(info.ImageUrl))
					{
						bitmap = UrlToImageMap[info.ImageUrl];
					}
					else
					{
						var imageUrl = new URL(info.ImageUrl);
						bitmap = BitmapFactory.DecodeStream(imageUrl.OpenStream());
						UrlToImageMap.Add(info.ImageUrl, bitmap);
					}
				}
				Application.SynchronizationContext.Post (_ => {
					info.ImageView.SetImageBitmap(bitmap);
				}, null);
			}
			catch (System.Exception)
			{
				// Log error, etc
			}
		}

		private class ImageDownloadInfo
		{

			public string ImageUrl
			{
				get;
				set;
			}

			public ImageView ImageView
			{
				get;
				set;
			}
		}
	}

}
