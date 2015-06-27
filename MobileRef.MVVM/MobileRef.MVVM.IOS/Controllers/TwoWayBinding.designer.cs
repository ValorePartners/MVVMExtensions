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
	[Register ("TwoWayBinding")]
	partial class TwoWayBinding
	{
		[Outlet]
		UIKit.UIButton btnNavigate { get; set; }

		[Outlet]
		UIKit.UIButton btnTableBinding { get; set; }

		[Outlet]
		UIKit.UILabel lblAlt { get; set; }

		[Outlet]
		UIKit.UILabel lblHello { get; set; }

		[Outlet]
		UIKit.UITextField txtAlt { get; set; }

		[Outlet]
		UIKit.UITextField txtHello { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnNavigate != null) {
				btnNavigate.Dispose ();
				btnNavigate = null;
			}

			if (btnTableBinding != null) {
				btnTableBinding.Dispose ();
				btnTableBinding = null;
			}

			if (lblHello != null) {
				lblHello.Dispose ();
				lblHello = null;
			}

			if (txtAlt != null) {
				txtAlt.Dispose ();
				txtAlt = null;
			}

			if (txtHello != null) {
				txtHello.Dispose ();
				txtHello = null;
			}

			if (lblAlt != null) {
				lblAlt.Dispose ();
				lblAlt = null;
			}
		}
	}
}
