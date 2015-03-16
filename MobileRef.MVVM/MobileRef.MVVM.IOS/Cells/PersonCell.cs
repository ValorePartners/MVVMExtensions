
using System;

using Foundation;
using UIKit;
using MobileRef.MVVM.Shared;

namespace MobileRef.MVVM.IOS
{
	public partial class PersonCell : UITableViewCell,IViewCell<Person>
	{
		public static readonly UINib Nib = UINib.FromName ("PersonCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PersonCell");

		public PersonCell (IntPtr handle) : base (handle)
		{
		}

		public static PersonCell Create ()
		{
			return (PersonCell)Nib.Instantiate (null, null) [0];
		}

		#region IViewCell implementation

		public void Bind (Person item)
		{
			lblDisplay.Text = item.FirstName + " " + item.LastName;
		}

		#endregion
	}
}

