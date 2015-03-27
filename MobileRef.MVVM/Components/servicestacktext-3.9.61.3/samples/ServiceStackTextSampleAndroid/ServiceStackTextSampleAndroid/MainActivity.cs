using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using ServiceStack.Text;

namespace ServiceStackTextSampleAndroid
{
	[Activity (Label = "ServiceStack.Text", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public static RootObject CurrentRootObject { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			FindViewById<Button> (Resource.Id.buttonParse).Click += async (sender, e) => {


				// Query the Yahoo weather api for the Boston MA forecast using yql
				var yql = System.Web.HttpUtility.UrlPathEncode ("select item from weather.forecast where location=\"USMA0046\"&format=json");

				var http = new HttpClient();
				var json = await http.GetStringAsync("http://query.yahooapis.com/v1/public/yql?q=" + yql);

				// Dump in console the recieved Json response
				Console.WriteLine (json);

				// Parse the Json into the POCO classes
				CurrentRootObject = JsonSerializer.DeserializeFromString<RootObject>(json);

				StartActivity(typeof(WeatherActivity));
			};

		}
	}
}


