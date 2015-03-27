using System;
using System.Drawing;
#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using MonoTouch.Dialog;

using ServiceStack.Text;

namespace ServiceStackTextSampleiOS
{
	public class DVCMenu : DialogViewController
	{
		WeatherViewController weatherViewController;
		public DVCMenu () : base (null, true)
		{
			Root = new RootElement ("ServiceStack Test") {
				new Section () {
					new StringElement ("Parse Weather API", ()=> {
						using (System.Net.WebClient wc = new System.Net.WebClient())
						{
							// Query the Yahoo weather api for the Boston MA forecast using yql
							var yql = System.Web.HttpUtility.UrlPathEncode ("select item from weather.forecast where location=\"USMA0046\"&format=json");
							string json = wc.DownloadString ("http://query.yahooapis.com/v1/public/yql?q=" + yql);

							// Dump in console the recieved Json response
							Console.WriteLine (json);

							// Parse the Json into the POCO classes
							var jsonInfo = JsonSerializer.DeserializeFromString<RootObject>(json);

							weatherViewController = new WeatherViewController (jsonInfo);
						}
						NavigationController.PushViewController (weatherViewController, true);
					})
				}
			};
		}
	}
}

