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
	[Register ("NOSqlData")]
	partial class NOSqlData
	{
		[Outlet]
		UIKit.UIButton btnSearch { get; set; }

		[Outlet]
		UIKit.UITableView lstPeople { get; set; }

		[Outlet]
		UIKit.UITextField txtSearch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtSearch != null) {
				txtSearch.Dispose ();
				txtSearch = null;
			}

			if (btnSearch != null) {
				btnSearch.Dispose ();
				btnSearch = null;
			}

			if (lstPeople != null) {
				lstPeople.Dispose ();
				lstPeople = null;
			}
		}
	}
}
