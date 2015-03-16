
using System;

using Foundation;
using UIKit;
using MobileRef.MVVM.Shared;

namespace MobileRef.MVVM.IOS
{
	public partial class WeatherCell : UITableViewCell,IViewCell<WeatherData>
	{
		public static readonly UINib Nib = UINib.FromName ("WeatherCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("WeatherCell");

		public WeatherCell (IntPtr handle) : base (handle)
		{
		}

		public static WeatherCell Create ()
		{
			return (WeatherCell)Nib.Instantiate (null, null) [0];
		}

		#region IViewCell implementation

		public void Bind (WeatherData item)
		{
			this.lblDescription.Text = item.description;

			var dtDateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds( item.dt ).ToLocalTime();
			lblDate.Text = dtDateTime.ToLongDateString();

			imgIcon.LoadFromURL (item.icon);
		}

		#endregion
	}
}

