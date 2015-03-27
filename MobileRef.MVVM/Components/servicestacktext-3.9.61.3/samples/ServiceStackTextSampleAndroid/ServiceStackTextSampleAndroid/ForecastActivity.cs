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

namespace ServiceStackTextSampleAndroid
{
	[Activity (Label = "Forecast")]			
	public class ForecastActivity : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.ListAdapter = new ForecastListAdapter (this, MainActivity.CurrentRootObject.Query.Results.Channel.Item.Forecast);
		}
	}

	public class ForecastListAdapter : BaseAdapter<Forecast>
	{
		public ForecastListAdapter(Activity context, List<Forecast> forecasts) : base()
		{
			Context = context;
			Forecasts = forecasts;
		}

		public Activity Context { get;set; }
		public List<Forecast> Forecasts { get; set; }

		public override long GetItemId (int position) { return position; }
		public override int Count { get { return Forecasts.Count; } }
		public override Forecast this [int index] { get { return Forecasts [index]; } }

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var f = Forecasts [position];

			var view = convertView ?? Context.LayoutInflater.Inflate (Resource.Layout.forecastitemlayout, parent, false);

			view.FindViewById<TextView> (Resource.Id.textViewDate).Text = f.Date;
			view.FindViewById<TextView> (Resource.Id.textViewCode).Text = f.Code.ToString ();
			view.FindViewById<TextView> (Resource.Id.textViewHigh).Text = f.High.ToString();
			view.FindViewById<TextView> (Resource.Id.textViewLow).Text = f.Low.ToString();
			view.FindViewById<TextView> (Resource.Id.textViewText).Text = f.Text;

			return view;
		}
	}
}

