using System;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Net;
using Android.Views;

namespace MobileRef.MVVM.Android
{
	public static class VPActivityExtensions
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
			
		public static void GoBack(this Activity context){
			context.OnBackPressed ();
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


}
