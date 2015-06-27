// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MobileRef.MVVM.IOS
{
	[Register ("MainView")]
	partial class MainView
	{
		[Outlet]
		UIKit.UIButton btnActionSheet { get; set; }

		[Outlet]
		UIKit.UIButton btnErrorAnalytics { get; set; }

		[Outlet]
		UIKit.UIButton btnInputDialog { get; set; }

		[Outlet]
		UIKit.UIButton btnLocalNotification { get; set; }

		[Outlet]
		UIKit.UIButton btnModalPopup { get; set; }

		[Outlet]
		UIKit.UIButton btnMultipleMessage { get; set; }

		[Outlet]
		UIKit.UIButton btnNavigate { get; set; }

		[Outlet]
		UIKit.UIButton btnNavigateParams { get; set; }

		[Outlet]
		UIKit.UIButton btnSimpleMessage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnErrorAnalytics != null) {
				btnErrorAnalytics.Dispose ();
				btnErrorAnalytics = null;
			}

			if (btnActionSheet != null) {
				btnActionSheet.Dispose ();
				btnActionSheet = null;
			}

			if (btnInputDialog != null) {
				btnInputDialog.Dispose ();
				btnInputDialog = null;
			}

			if (btnLocalNotification != null) {
				btnLocalNotification.Dispose ();
				btnLocalNotification = null;
			}

			if (btnModalPopup != null) {
				btnModalPopup.Dispose ();
				btnModalPopup = null;
			}

			if (btnMultipleMessage != null) {
				btnMultipleMessage.Dispose ();
				btnMultipleMessage = null;
			}

			if (btnNavigate != null) {
				btnNavigate.Dispose ();
				btnNavigate = null;
			}

			if (btnNavigateParams != null) {
				btnNavigateParams.Dispose ();
				btnNavigateParams = null;
			}

			if (btnSimpleMessage != null) {
				btnSimpleMessage.Dispose ();
				btnSimpleMessage = null;
			}
		}
	}
}
