using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioToolbox;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using SystemConfiguration;
using UIKit;

namespace MobileRef.MVVM.IOS
{
	// EXTENSIONS FOR IOS UIViewController
	public static class ControllerExtensions
	{
		public static void ShowMessage (this UIViewController controller, string title, string message)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create ("Okay", UIAlertActionStyle.Default, null));
			controller.PresentViewController (alert, true, null);
		}

		public static void ShowMessage (this UIViewController controller, string title, string message, string button1Text, Action button1)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create (button1Text, UIAlertActionStyle.Default, action => {
				button1.Invoke ();
			}));
			controller.PresentViewController (alert, true, null);
		}

		public static void ShowMessage (this UIViewController controller, string title, string message, string button1Text, Action button1, string button2Text, Action button2)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create (button1Text, UIAlertActionStyle.Default, action => {
				button1.Invoke ();
			}));
			alert.AddAction (UIAlertAction.Create (button2Text, UIAlertActionStyle.Default, action => {
				button2.Invoke ();
			}));
			controller.PresentViewController (alert, true, null);
		}

		public static void ShowMessage (this UIViewController controller, string title, string message, string button1Text, Action button1, string button2Text, Action button2, string button3Text, Action button3)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create (button1Text, UIAlertActionStyle.Default, action => {
				button1.Invoke ();
			}));
			alert.AddAction (UIAlertAction.Create (button2Text, UIAlertActionStyle.Default, action => {
				button2.Invoke ();
			}));
			alert.AddAction (UIAlertAction.Create (button3Text, UIAlertActionStyle.Default, action => {
				button3.Invoke ();
			}));
			controller.PresentViewController (alert, true, null);
		}

		public static void ShowMessage (this UIViewController controller, string title, string message, string buttonText, string placeHolder, UIKeyboardType keyboardType, Action<string> resultAction)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create (buttonText, UIAlertActionStyle.Default, action => {
				var result = alert.TextFields [0].Text;
				resultAction.Invoke (result);
			}));
			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Default, null));
			alert.AddTextField ((field) => {
				field.Placeholder = placeHolder;
			});
			alert.TextFields [0].KeyboardType = keyboardType;

			controller.PresentViewController (alert, true, null);
		}

		public static void ShowActionSheet (this UIViewController controller, CGRect controlFrame, string title, string subTitle, string[] list, Action<string> callBack)
		{
			// Create a new Alert Controller
			UIAlertController alert = UIAlertController.Create (title, subTitle, UIAlertControllerStyle.ActionSheet);
			alert.View.TintColor = UIColor.Black;
			foreach (var obj in list) {
				alert.AddAction (UIAlertAction.Create (obj, UIAlertActionStyle.Default, (action) => {
					callBack.Invoke (action.Title);
				}));
			}
			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, (action) => {
				//Console.WriteLine("Cancel button pressed.");
			}));
	
			var presentationPopover = alert.PopoverPresentationController;
			if (presentationPopover != null) {
				presentationPopover.SourceView = controller.View;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
				presentationPopover.SourceRect = controlFrame;
			}
			controller.PresentViewController (alert, true, null);
		}

		public static void ShowLocalNotification (this UIViewController controller, string msg, nint badgeCount, double fromSeconds, string categoryName)
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes (
				               UIUserNotificationType.Alert
				               | UIUserNotificationType.Badge
				               | UIUserNotificationType.Sound,
				               new NSSet ());
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);

			var category = string.IsNullOrEmpty (categoryName) ? "default_Id" : categoryName;
			var notification = new UILocalNotification () {
				AlertBody = msg,
				AlertAction = null,
				ApplicationIconBadgeNumber = badgeCount,
				Category = category,
				FireDate = NSDate.FromTimeIntervalSinceNow (fromSeconds)
			};
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

		public static void ShowLocalNotification (this UIViewController controller, string msg, string imageFile, nint badgeCount, double fromSeconds, string categoryName)
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes (
				               UIUserNotificationType.Alert
				               | UIUserNotificationType.Badge
				               | UIUserNotificationType.Sound,
				               new NSSet ());
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);

			var category = string.IsNullOrEmpty (categoryName) ? "default_Id" : categoryName;
			var notification = new UILocalNotification () {
				AlertBody = msg,
				AlertAction = null,
				AlertLaunchImage = imageFile,
				ApplicationIconBadgeNumber = badgeCount,
				Category = category,
				FireDate = NSDate.FromTimeIntervalSinceNow (fromSeconds)
			};
		
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

		public static void ShowLocalNotification (this UIApplicationDelegate appDelegate, string msg, nint badgeCount, double fromSeconds, string categoryName)
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes (
				               UIUserNotificationType.Alert
				               | UIUserNotificationType.Badge
				               | UIUserNotificationType.Sound,
				               new NSSet ());
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);

			var category = string.IsNullOrEmpty (categoryName) ? "default_Id" : categoryName;
			var notification = new UILocalNotification () {
				AlertBody = msg,
				AlertAction = null,
				ApplicationIconBadgeNumber = badgeCount,
				Category = category,
				FireDate = NSDate.FromTimeIntervalSinceNow (fromSeconds)
			};
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

		public static void ShowLocalNotification (this UIApplicationDelegate appDelegate, string msg, string imageFile, nint badgeCount, double fromSeconds, string categoryName)
		{
			var settings = UIUserNotificationSettings.GetSettingsForTypes (
				               UIUserNotificationType.Alert
				               | UIUserNotificationType.Badge
				               | UIUserNotificationType.Sound,
				               new NSSet ());
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);

			var category = string.IsNullOrEmpty (categoryName) ? "default_Id" : categoryName;
			var notification = new UILocalNotification () {
				AlertBody = msg,
				AlertAction = null,
				AlertLaunchImage = imageFile,
				ApplicationIconBadgeNumber = badgeCount,
				Category = category,
				FireDate = NSDate.FromTimeIntervalSinceNow (fromSeconds)
			};

			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

		public static void ShowMessage (this UIApplicationDelegate appDelegate, string title, string message)
		{
			var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create ("Okay", UIAlertActionStyle.Default, null));
			appDelegate.Window.RootViewController.PresentViewController (alert, true, null);
		}

		public static Nullable<T> ToPrimitive<T> (this UITextField txt)where T: struct
		{
			if (!string.IsNullOrEmpty (txt.Text)) {
				return TConverter.ChangeType<T> (txt.Text);  
			}
			return null;
		}

		public static void PlaySoundFile (this UIViewController controller, string filePath)
		{
			var url = NSUrl.FromFilename (filePath);
			var newSound = new SystemSound (url);
			newSound.PlaySystemSound ();
		}

		public static void PushController (this UIApplicationDelegate appDelegate, string controllerName)
		{
			var nav = appDelegate.Window.RootViewController as UINavigationController;
			var ctrl = (UIViewController)nav.Storyboard.InstantiateViewController (controllerName);
			nav.PushViewController (ctrl, true);
		}

		public static void PushController (this UIApplicationDelegate appDelegate, string controllerName, string propName, object objValue)
		{
			var nav = appDelegate.Window.RootViewController as UINavigationController;
			var ctrl = (UIViewController)nav.Storyboard.InstantiateViewController (controllerName);
			var prop = ctrl.GetType ().GetProperty (propName);
			if (prop != null)
				prop.SetValue (ctrl, objValue);
			nav.PushViewController (ctrl, true);
		}

		public static void PushController (this UIViewController controller, string controllerName)
		{
			var ctrl = (UIViewController)controller.Storyboard.InstantiateViewController (controllerName);
			controller.NavigationController.PushViewController (ctrl, true);
		}


		public static void PushController (this UIViewController controller, string controllerName, string propName, object objValue)
		{
			var ctrl = (UIViewController)controller.Storyboard.InstantiateViewController (controllerName);
			var prop = ctrl.GetType ().GetProperty (propName);
			if (prop != null)
				prop.SetValue (ctrl, objValue);
			controller.NavigationController.PushViewController (ctrl, true);
		
		}


		public static void ModalController (this UIViewController controller, string controllerName)
		{
			var ctrl = (UIViewController)controller.Storyboard.InstantiateViewController (controllerName);
			controller.NavigationController.PresentViewController (ctrl, true, null);
		}

		public static T ModalController<T> (this UIViewController controller)
			where T: UIViewController
		{
			var ctrl = (T)controller.Storyboard.InstantiateViewController (typeof(T).Name);
			controller.NavigationController.PresentViewController (ctrl, true, null);
			return ctrl;
		}

		public static void RotateView (this UIViewController controller, UIInterfaceOrientation orientation)
		{
			var i = (int)orientation;
			var n = new NSNumber (nfloat.Parse (i.ToString ()));
			UIDevice.CurrentDevice.SetValueForKey (n, new NSString ("orientation"));
		}

		public static void PerformCall (this UIViewController controller, string phoneNumber)
		{
			var phNum = new NSUrl (string.Format ("tel:{0}", phoneNumber));
			if (UIApplication.SharedApplication.CanOpenUrl (phNum)) {
				UIApplication.SharedApplication.OpenUrl (phNum);
			} else {
				controller.ShowMessage ("Unsupported", "Phone calls are not supported on this device");
			}
		}

		public static void PerformSMS (this UIViewController controller, string phoneNumber)
		{
			var smsTo = new NSUrl (string.Format ("sms:{0}", phoneNumber));
			if (UIApplication.SharedApplication.CanOpenUrl (smsTo)) {
				UIApplication.SharedApplication.OpenUrl (smsTo);
			} else {
				controller.ShowMessage ("Unsupported", "SMS is not supported on this device");
			}
		}

		public static void PerformEmail (this UIViewController controller, string emailAddress, string subject, string message)
		{
			var smsTo = new NSUrl (string.Format ("mailto:{0}?subject={1}&body={2}", emailAddress, subject, message));
			if (UIApplication.SharedApplication.CanOpenUrl (smsTo)) {
				UIApplication.SharedApplication.OpenUrl (smsTo);
			} else {
				controller.ShowMessage ("Unsupported", "Email is not supported on this device");
			}
		}

		public static void PerformMap (this UIViewController controller, string streetAddress)
		{
			//Apple documentation: https://developer.apple.com/library/iad/featuredarticles/iPhoneURLScheme_Reference/iPhoneURLScheme_Reference.pdf

			string urlString = @"http://maps.apple.com/?daddr=" + streetAddress.Replace (' ', '+') + "&saddr=my+location";
			var mapUrl = new NSUrl (urlString);
			if (UIApplication.SharedApplication.CanOpenUrl (mapUrl)) {
				UIApplication.SharedApplication.OpenUrl (mapUrl);
			} else {
				controller.ShowMessage ("Unsupported", "Map navigation is not supported on this device");
			}
		}

		public static void PerformMap (this UIViewController controller, double latitude, double longtitude)
		{
			string urlString = string.Format ("http://maps.apple.com/?daddr={0},{1}&saddr=my+location", latitude, longtitude);
			var mapUrl = new NSUrl (urlString);
			if (UIApplication.SharedApplication.CanOpenUrl (mapUrl)) {
				UIApplication.SharedApplication.OpenUrl (mapUrl);
			} else {
				controller.ShowMessage ("Unsupported", "Map navigation is not supported on this device");
			}
		}

		public static CGRect CopyFrame (this UIView view, nfloat? x = null, nfloat? y = null)
		{
			if (x.HasValue && !y.HasValue) {
				return new CGRect (x.Value, view.Frame.Y, view.Frame.Width, view.Frame.Height);
			} else if (!x.HasValue && y.HasValue) {
				return new CGRect (view.Frame.X, y.Value, view.Frame.Width, view.Frame.Height);
			} else if (x.HasValue && y.HasValue) {
				return new CGRect (x.Value, y.Value, view.Frame.Width, view.Frame.Height);
			} else {
				return new CGRect (view.Frame.X, view.Frame.Y, view.Frame.Width, view.Frame.Height);
			}
		}

		public static UIImage Resize (this UIImage img, float newHeight)
		{
			var percent = newHeight / img.Size.Height;
			var newWidth = percent * img.Size.Width;
			var newSize = new CGSize (newWidth, newHeight);
			UIGraphics.BeginImageContextWithOptions (newSize, false, UIScreen.MainScreen.Scale);
			img.Draw (new CGRect (0, 0, newSize.Width, newSize.Height));
			img = UIGraphics.GetImageFromCurrentImageContext ();
			return img;
		}

		public static UIImage ChangeColor (this UIImage image, UIColor color)
		{
			var rect = new  CGRect (0, 0, image.Size.Width, image.Size.Height);
			UIGraphics.BeginImageContext (rect.Size);
			var ctx = UIGraphics.GetCurrentContext ();
			ctx.ClipToMask (rect, image.CGImage);
			ctx.SetFillColor (color.CGColor);
			ctx.FillRect (rect);
			var img = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return UIImage.FromImage (img.CGImage, 1.0f, UIImageOrientation.DownMirrored);
		}

		public static UIColor HexToColor (this int hexValue)
		{
			return UIColor.FromRGB (
				(((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
				(((float)(hexValue & 0xFF)) / 255.0f)
			);
		}

		public static void ApplyDropShadow (this UIView view)
		{
			view.Layer.ShadowColor = (UIColor.DarkGray).CGColor;
			view.Layer.ShadowOffset = new CGSize (0, 1);
			view.Layer.ShadowOpacity = 1;
			view.Layer.ShadowRadius = 1;
		}


		/// <summary>
		/// Load image async when instead of a UITableView Cell
		/// </summary>
		/// <param name="imgView">Image view.</param>
		/// <param name="uri">URI.</param>
		/// <param name="view">View.</param>
		/// <param name="path">Path.</param>
		public static async void LoadFromURL (this UIImageView imgView, string uri, UITableView view, NSIndexPath path)
		{
			try {
				await Task.Factory.StartNew (() => {
					using (var url = new NSUrl (uri)) {
						using (var data = NSData.FromUrl (url)) {
							imgView.InvokeOnMainThread (() => {
								imgView.Image = UIImage.LoadFromData (data);
								view.ReloadRows (new NSIndexPath[]{ path }, UITableViewRowAnimation.None);
							});
						}
					}
				});
			} catch (Exception) {
				//do nothing
			}
		}

		public static async void LoadFromURL (this UIImageView imgView, string uri)
		{
			try {
				await Task.Factory.StartNew (() => {
					using (var url = new NSUrl (uri)) {
						using (var data = NSData.FromUrl (url)) {
							imgView.InvokeOnMainThread (() => {
								imgView.Image = UIImage.LoadFromData (data);
							});
						}
					}
				});
			} catch (Exception) {
				//do nothing
			}
		}

		public static void AddPadding (this UITextField control)
		{
			var padding = new UIView (new CGRect (0, 0, 10, 20));
			control.LeftView = padding;
			control.LeftViewMode = UITextFieldViewMode.Always;
		}

	}





	//THIS SECTION CONTAINS SETTINGS, INTERFACES, ATTRIBUTES, UITableViewSource & BASE CLASSES FOR THE EXTENSIONS
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

	public interface IHandlers
	{
		void ViewEventHandler (object e, EventArgs args);

		void ControlsHandler (object sender, EventArgs args);
	}

	public class Ctrl
	{
		public static List<string> ControlList = new List<string> (){ "UIButton" };

		public static void HookUIControl (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				switch (contrlType) {
				case "UIButton":
					((UIButton)ctrl).TouchUpInside += handler.ControlsHandler;
					break;
				}
			}
		}

		public static void UnHookUIControll (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				switch (contrlType) {
				case "UIButton":
					((UIButton)ctrl).TouchUpInside -= handler.ControlsHandler;
					break;
				}
			}
		}

	}

	public interface IViewCell<T>
	{
		void Bind (T item);
	}

	public interface IBaseSource
	{
		void UpdateCollection (ICollection collection);

		UITableView TableView{ get; set; }

		object SelectedItem{ get; set; }

		event EventHandler RowSelectedEvent;

		nint Tag{ get; set; }
	}

	public class TableSource<T, K> :UITableViewSource, IBaseSource
		where T:class
		where K: IViewCell<T>
	{

		public event EventHandler RowSelectedEvent;

		public nint Tag{ get; set; }

		public T[] Items{ get; set; }

		public object SelectedItem{ get; set; }

		string cellIdentifier;

		public UITableView TableView { get; set; }

		public TableSource (T[] items, string cellId)
		{
			Items = items;
			cellIdentifier = cellId;
		}

		public void UpdateCollection (ICollection collection)
		{
			var List = new List<T> ();
			foreach (var item in collection) {
				List.Add ((T)item);
			}
			Items = List.ToArray ();
			TableView.ReloadData ();
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Items.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			var cell = tableView.DequeueReusableCell (cellIdentifier);
			if (cell == null) {
				var cellName = typeof(K).Name;
				var obj = UINib.FromName (cellName, NSBundle.MainBundle).Instantiate (null, null) [0];
				cell = (UITableViewCell)obj;
			}
			((IViewCell<T>)cell).Bind (Items [indexPath.Row]);
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			SelectedItem = Items [indexPath.Row];
			if (RowSelectedEvent != null) {
				RowSelectedEvent (this, new EventArgs ());
			}
		}
	}

	public static class TConverter
	{
		public static T ChangeType<T> (object value)
		{
			return (T)ChangeType (typeof(T), value);
		}

		public static object ChangeType (Type t, object value)
		{
			var tc = TypeDescriptor.GetConverter (t);
			return tc.ConvertFrom (value);
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

			switch (typeName) {
			case "Int32":
				int intValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!int.TryParse (stringValue, out intValue))
						return null;
				} else {
					intValue = 0;
				}
				return intValue;
				break;
			case "Boolean":
				bool boolValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!bool.TryParse (stringValue, out boolValue))
						return null;
				} else {
					boolValue = false;
				}
				return boolValue;
				break;
			case "Single":
				float floatValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!float.TryParse (stringValue, out floatValue))
						return null;
				} else {
					floatValue = 0.0f;
				}
				return floatValue;
				break;
			case "Double":
				double doubleValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!double.TryParse (stringValue, out doubleValue))
						return null;
				} else {
					doubleValue = 0.0d;
				}
				return doubleValue;
				break;
			case "DateTime":
				DateTime dtValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!DateTime.TryParse (stringValue, out dtValue))
						return null;
				} else {
					return null;
				}
				return dtValue;
				break;
			default:
				return stringValue;
				break;
			}

		}
	}



	// THIS IS THE REACHABILITY PORTION OF THE CODE
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}

	public static class Reachability
	{
		public static string HostName = "www.google.com";

		public static void Start ()
		{
			var remoteStatus = Reachability.RemoteHostStatus ();
			var internetStatus = Reachability.InternetConnectionStatus ();
			var wifiStatus = Reachability.LocalWifiConnectionStatus ();
			Reachability.ReachabilityChanged += (object sender, EventArgs e) => {
				var rhStatus = Reachability.RemoteHostStatus ();
				var iStatus = Reachability.InternetConnectionStatus ();
				var wiStatus = Reachability.LocalWifiConnectionStatus ();
				if (rhStatus == NetworkStatus.NotReachable && iStatus == NetworkStatus.NotReachable && wiStatus == NetworkStatus.NotReachable) {

//					if (AppData.IsConnected) {
//						AppData.IsConnected = false;
//					}

				} else {
					//AppData.IsConnected = true;
				}
			};
			if (remoteStatus == NetworkStatus.NotReachable && internetStatus == NetworkStatus.NotReachable && wifiStatus == NetworkStatus.NotReachable) {
//				if (AppData.IsConnected) {
//					AppData.IsConnected = false;
//				}
			} else {
				//AppData.IsConnected = true;
			}

		}

		public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

			// Since the network stack will automatically try to get the WAN up,
			// probe that
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				noConnectionRequired = true;

			return isReachable && noConnectionRequired;
		}

		// Is the host reachable with the current network configuration
		public static bool IsHostReachable (string host)
		{
			if (host == null || host.Length == 0)
				return false;

			using (var r = new NetworkReachability (host)) {
				NetworkReachabilityFlags flags;

				if (r.TryGetFlags (out flags)) {
					return IsReachableWithoutRequiringConnection (flags);
				}
			}
			return false;
		}

		//
		// Raised every time there is an interesting reachable event,
		// we do not even pass the info as to what changed, and
		// we lump all three status we probe into one
		//
		public static event EventHandler ReachabilityChanged;

		static void OnChange (NetworkReachabilityFlags flags)
		{
			var h = ReachabilityChanged;
			if (h != null)
				h (null, EventArgs.Empty);
		}

		//
		// Returns true if it is possible to reach the AdHoc WiFi network
		// and optionally provides extra network reachability flags as the
		// out parameter
		//
		static NetworkReachability adHocWiFiNetworkReachability;

		public static bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null) {
				adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte[] { 169, 254, 0, 0 }));
				adHocWiFiNetworkReachability.SetNotification (OnChange);
				//adHocWiFiNetworkReachability.SetCallback(OnChange);
				adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			if (!adHocWiFiNetworkReachability.TryGetFlags (out flags))
				return false;

			return IsReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability defaultRouteReachability;

		static bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null) {
				defaultRouteReachability = new NetworkReachability (new IPAddress (0));
				defaultRouteReachability.SetNotification (OnChange);
				defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			if (!defaultRouteReachability.TryGetFlags (out flags))
				return false;
			return IsReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability remoteHostReachability;

		public static NetworkStatus RemoteHostStatus ()
		{
			NetworkReachabilityFlags flags;
			bool reachable;

			if (remoteHostReachability == null) {
				remoteHostReachability = new NetworkReachability (HostName);

				// Need to probe before we queue, or we wont get any meaningful values
				// this only happens when you create NetworkReachability from a hostname
				reachable = remoteHostReachability.TryGetFlags (out flags);

				remoteHostReachability.SetNotification (OnChange);
				remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			} else
				reachable = remoteHostReachability.TryGetFlags (out flags);

			if (!reachable)
				return NetworkStatus.NotReachable;

			if (!IsReachableWithoutRequiringConnection (flags))
				return NetworkStatus.NotReachable;

			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;

			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus InternetConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0)) {
				return NetworkStatus.NotReachable;
			} else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;
			else if (flags == 0)
				return NetworkStatus.NotReachable;
			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus LocalWifiConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable (out flags)) {
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return NetworkStatus.ReachableViaWiFiNetwork;
			}
			return NetworkStatus.NotReachable;
		}
	}

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

			var ctrl = pbs.FirstOrDefault (x => x.Tag == tag);
			if (ctrl != null) {
				var prop = sender.GetType ().GetProperty (ctrl.BindingProperty, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
				var propValue = prop.GetValue (sender, null);
				var vmProp = viewModel.GetType ().GetProperty (ctrl.ViewModelProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

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
			var tag = ((UIControl)sender).Tag;
			var ctrl = ebs.FirstOrDefault (x => x.Tag == tag);

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
								var ib = ctrlProp.PropertyType.GetInterface ("IBaseSource");
								if (ib != null) {
									var ctrlInstance = ctrlProp.GetValue (controller);
									((IBaseSource)ctrlInstance).UpdateCollection ((ICollection)sender);
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
					var ctrlInstaceProp = ctrlInstance.GetType ().GetProperty (obj.BindingProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var currentValue = ctrlInstaceProp.GetValue (ctrlInstance);
					var vmProp = sender.GetType ().GetProperty (obj.ViewModelProperty,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					var vmValue = vmProp.GetValue (sender, null);

					if (ctrlInstance is IBaseSource) {
						((IBaseSource)ctrlInstance).UpdateCollection ((ICollection)vmValue);

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

					var eventInstance = instance.GetType ().GetEvent (setting.EventName,BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
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
//			var exp = ((MemberExpression)propertyRefExpr.Body);
//			return ((MemberExpression)exp.Expression).Member.Name;

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
				if (Ctrl.ControlList.Contains (pt.Name)) {

					if (!ebs.Any (x => x.BindingObject == pn)) {
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
				if (Ctrl.ControlList.Contains (pt.Name)) {
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
	}
}

