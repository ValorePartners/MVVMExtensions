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
	[Register ("WeatherCell")]
	partial class WeatherCell
	{
		[Outlet]
		UIKit.UIImageView imgIcon { get; set; }

		[Outlet]
		UIKit.UILabel lblDate { get; set; }

		[Outlet]
		UIKit.UILabel lblDescription { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgIcon != null) {
				imgIcon.Dispose ();
				imgIcon = null;
			}

			if (lblDescription != null) {
				lblDescription.Dispose ();
				lblDescription = null;
			}

			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}
		}
	}
}
