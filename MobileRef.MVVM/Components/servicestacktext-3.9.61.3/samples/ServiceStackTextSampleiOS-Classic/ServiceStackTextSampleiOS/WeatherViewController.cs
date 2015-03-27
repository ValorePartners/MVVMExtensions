using System;
using System.Drawing;
using System.Linq;
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
	public class WeatherViewController : DialogViewController
	{
		// We use MonoTouch.Dialog to visually represent the parsed json data
		// http://docs.xamarin.com/guides/ios/user_interface/monotouch.dialog/
		public WeatherViewController (RootObject json) : base (null, true)
		{
			var item = json.Query.Results.Channel.Item;

			Root = new RootElement ("Weather in Boston, MA") {
				new Section (item.Title) {
					new StyledStringElement ("Latitude", item.Lat, UITableViewCellStyle.Subtitle),
					new StyledStringElement ("Longitude", item.Lon, UITableViewCellStyle.Subtitle),
					new RootElement ("Current Condition") {
						new Section () {
							new StyledStringElement ("Code", item.Condition.Code, UITableViewCellStyle.Subtitle),
							new StyledStringElement ("Date", item.Condition.Date, UITableViewCellStyle.Subtitle),
							new StyledStringElement ("Temp", item.Condition.Temp, UITableViewCellStyle.Subtitle),
							new StyledStringElement ("Text", item.Condition.Text, UITableViewCellStyle.Subtitle)
						}
					},
					new RootElement ("Forecast") {
						item.Forecast.Where (f => f != null).Select (f => {
							return new Section (f.Day + ", " + f.Date) {
								new StyledStringElement ("Code", f.Code, UITableViewCellStyle.Subtitle),
								new StyledStringElement ("High", f.High, UITableViewCellStyle.Subtitle),
								new StyledStringElement ("Low", f.Low, UITableViewCellStyle.Subtitle),
								new StyledStringElement ("Text", f.Text, UITableViewCellStyle.Subtitle)
							};
						})
					},
					new HtmlElement ("Link to site", item.Link),
					new StringElement ("Description", ()=> {
						var webViewController = new UIViewController ();
						var webView = new UIWebView (View.Frame);
						webView.LoadHtmlString (item.Description, null);
						webViewController.View.Add (webView);
						NavigationController.PushViewController (webViewController, true);
					}),
					new RootElement ("Guid") {
						new Section () {
							new StyledStringElement ("IsPermaLink", item.Guid.IsPermaLink, UITableViewCellStyle.Subtitle),
							new StyledStringElement ("Content", item.Guid.Content, UITableViewCellStyle.Subtitle)
						}
					}
				},
				new Section () {
					new RootElement ("Show Parsed Json") {
						new Section () {
							new MultilineElement (json.Dump ())
						}
					}
				}
			};
		}
	}
}