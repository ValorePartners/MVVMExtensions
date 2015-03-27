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
	[Activity (Label = "Current Conditions")]			
	public class CurrentConditionsActivity : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var conditions = new List<Condition> ();
			conditions.Add (MainActivity.CurrentRootObject.Query.Results.Channel.Item.Condition);

			this.ListAdapter = new CurrentConditionsListAdapter (this, conditions);
		}
	}

	public class CurrentConditionsListAdapter : BaseAdapter<Condition>
	{
		public CurrentConditionsListAdapter(Activity context, List<Condition> conditions) : base()
		{
			Context = context;
			Conditions = conditions;
		}

		public Activity Context { get;set; }
		public List<Condition> Conditions { get; set; }

		public override long GetItemId (int position) { return position; }
		public override int Count { get { return Conditions.Count; } }
		public override Condition this [int index] { get { return Conditions [index]; } }

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var c = Conditions [position];

			var view = convertView ?? Context.LayoutInflater.Inflate (Resource.Layout.currentconditionsitemlayout, parent, false);

			view.FindViewById<TextView> (Resource.Id.textViewDate).Text = c.Date;
			view.FindViewById<TextView> (Resource.Id.textViewCode).Text = c.Code.ToString ();
			view.FindViewById<TextView> (Resource.Id.textViewTemp).Text = c.Temp.ToString();
			view.FindViewById<TextView> (Resource.Id.textViewText).Text = c.Text;

			return view;
		}
	}
}

