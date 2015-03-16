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
	[Register ("PersonCell")]
	partial class PersonCell
	{
		[Outlet]
		UIKit.UILabel lblDisplay { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblDisplay != null) {
				lblDisplay.Dispose ();
				lblDisplay = null;
			}
		}
	}
}
