using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;

namespace ServiceStackTextSampleAndroid
{
	[Activity (Label = "WeatherActivity")]			
	public class WeatherActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.weatheractivitylayout);

			var item = MainActivity.CurrentRootObject.Query.Results.Channel.Item;

			FindViewById<TextView> (Resource.Id.textViewLatitude).Text = item.Lat;
			FindViewById<TextView> (Resource.Id.textViewLongitude).Text = item.Lon;
			FindViewById<TextView> (Resource.Id.textViewLocation).Text = item.Title;


			FindViewById<Button>(Resource.Id.buttonForecast).Click += delegate {
				StartActivity(typeof(ForecastActivity));
			};

			FindViewById<Button>(Resource.Id.buttonCurrentConditions).Click += delegate {
				StartActivity(typeof(CurrentConditionsActivity));
			};

			FindViewById<Button>(Resource.Id.buttonDescription).Click += delegate {
				var intent = new Intent(this, typeof(HtmlActivity));
				intent.PutExtra("TITLE", "Description");
				intent.PutExtra("HTML", "<html><body><pre>" + item.Description + "</pre></body></html>");
				StartActivity(intent);
			};

			FindViewById<Button>(Resource.Id.buttonLinkToSite).Click += delegate {
				var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(item.Link));
				StartActivity(browserIntent);
			};

			FindViewById<Button>(Resource.Id.buttonShowJson).Click += delegate {
				var intent = new Intent(this, typeof(HtmlActivity));
				intent.PutExtra("TITLE", "JSON");
				intent.PutExtra("HTML", MainActivity.CurrentRootObject.Dump());
				intent.PutExtra("PLAIN", true);
				StartActivity(intent);
			};

			FindViewById<Button>(Resource.Id.buttonGuid).Click += delegate {
				var html = string.Format("<html><body><p><strong>{0}</strong>: {1}</p> <p><strong>{2}</strong>: {3}</p></body></html>",
					"IsPermaLink", item.Guid.IsPermaLink, "Content", item.Guid.Content);

				var intent = new Intent(this, typeof(HtmlActivity));
				intent.PutExtra("TITLE", "Guid");
				intent.PutExtra("HTML", html);
				StartActivity(intent);
			};
		}
	}
}

