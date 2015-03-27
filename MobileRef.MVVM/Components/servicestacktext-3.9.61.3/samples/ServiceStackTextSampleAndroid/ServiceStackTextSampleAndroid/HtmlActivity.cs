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
using Android.Webkit;

namespace ServiceStackTextSampleAndroid
{
	[Activity (Label = "HtmlActivity")]			
	public class HtmlActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.htmlactivitylayout);

			Title = Intent.GetStringExtra ("TITLE") ?? "";

			var html = Intent.GetStringExtra ("HTML") ?? "";

			var webView = FindViewById<WebView> (Resource.Id.webView);

			var plain = Intent.GetBooleanExtra ("PLAIN", false);

			webView.LoadData (html, plain ? "text/plain" : "text/html", "UTF8");
		}
	}
}

