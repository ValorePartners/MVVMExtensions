using System;
using System.Reflection;
using AudioToolbox;
using CoreGraphics;
using Foundation;
using UIKit;

namespace $rootnamespace$
{
	public static class VPControllerExtensions
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
			
	}

}
