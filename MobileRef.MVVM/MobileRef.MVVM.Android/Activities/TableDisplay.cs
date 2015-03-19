using System;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileRef.MVVM.Shared;
using System.Collections.Generic;


namespace MobileRef.MVVM.Android
{
	[Activity (Label = "TableDisplay")]			
	public class TableDisplay : Activity,IHandlers
	{

		public EditText txtSearch{ get; private set; }
		public Button btnSearch{ get; private set; }
		public ListView lstWeather{ get; private set; }

		public WeatherViewModel VM{ get; set;}
		public BindableHUD Progress{ get; private set;}
		private BindingManager<TableDisplay,WeatherViewModel> bind;

		//Two ways to create and use a BaseAdapter
		//public HolderAdapter<WeatherData,WeatherDataHolder> Adapter{ get;  set; }
		public CustomSource Adapter{ get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Progress = new BindableHUD (this);
			VM = AppData.WeatherVM;
			SetContentView (Resource.Layout.TableDisplay);
			this.CreateControls<TableDisplay> ();
			bind = new BindingManager<TableDisplay, WeatherViewModel> ();
			bind.BindProperty (() => txtSearch.Text, () => VM.SearchLocation);
			bind.BindProperty (() => Progress.Message, () => VM.LoadMessage);
			bind.BindProperty (() => Progress.Visible, () => VM.IsLoading);
			bind.BindCommand (() => btnSearch, ()=>VM.PerformSearch);
		
			//Two ways to create and use a BaseAdapter
			//Adapter = new HolderAdapter<WeatherData, WeatherDataHolder> (this.LayoutInflater, VM.WeatherCollection.ToArray ());
			Adapter = new CustomSource (VM.WeatherCollection.ToArray (), this.LayoutInflater);


			lstWeather.Adapter = Adapter;
			bind.BindProperty (() => lstWeather.SelectedItem, () => VM.SelectedWeather);
			bind.BindProperty (() => Adapter.Items, () => VM.WeatherCollection);
		}

		protected override void OnResume ()
		{
			bind.RegisterBindingEvents (this, VM);
			base.OnResume ();
		}
		protected override void OnPause ()
		{
			bind.UnRegisterBindingEvents (this);
			AppData.SaveWeatherVM ();
			base.OnPause ();
		}

		#region IHandlers implementation

		public void ViewEventHandler (object e, System.EventArgs args)
		{
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				switch (propName) {
				case "SelectedWeather":
					this.ShowMessage ("Selected", VM.SelectedWeather.description);
					break;
				case "ExceptionOccurred":
					this.ShowMessage ("Nasty Error", VM.ExceptionOccurred.Message);
					break;
				}
			}
		}

		public void ControlsHandler (object sender, System.EventArgs args)
		{
//			if (sender is ListView) {
//				var position = ((global::Android.Widget.AdapterView.ItemClickEventArgs)args).Position;
//				var item = VM.WeatherCollection [position];
//				this.ShowMessage ("Selected", item.description);
//			}
		}

		#endregion


	}


	//This is an example of using the ViewHolder pattern with the HolderAdapter
//	public class WeatherDataHolder:ViewHolder<WeatherData>{
//
//		public TextView txtDescription{ get; private set; }
//		public TextView txtDate{ get; private set; }
//		public ImageView imgWeather{ get; private set; }
//	
//
//		public override void Init (View view)
//		{
//
//			this.CreateControls<WeatherDataHolder> (view);
//		}
//
//		public override void Populate (WeatherData item)
//		{
//			txtDescription.Text = item.description;
//
//			var dtDateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
//			dtDateTime = dtDateTime.AddSeconds( item.dt ).ToLocalTime();
//			txtDate.Text = dtDateTime.ToLongDateString();
//
//			ImageDownloader.AssignImageAsync(imgWeather, item.icon);
//		}
//
//		public override int CellResourceId {
//			get {
//				return Resource.Layout.WeatherCell;
//			}
//		}
//
//	}


	//Explicit way to create a UITableViewSource. Databinding must use the IBaseAdapter
	public class CustomSource:BaseAdapter<WeatherData>,IBaseAdapter{

		public WeatherData[] Items{ get; set; }
		private LayoutInflater inflator;
		public CustomSource (WeatherData[] items, LayoutInflater inflator)
		{
			this.Items = items;
			this.inflator = inflator;
		}
	
		public void UpdateCollection (System.Collections.ICollection collection)
		{
//			var List = new List<WeatherData> ();
//			foreach (var item in collection) {
//				List.Add ((WeatherData)item);
//			}
//			Items = List.ToArray ();
			Items = collection.ToArray<WeatherData>();
			this.NotifyDataSetChanged ();
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			var item = Items [position];
			var view = convertView;
			if (view == null) {
				view = inflator.Inflate (Resource.Layout.WeatherCell, null);
			} 
			var txtDescription = view.FindViewById<TextView> (Resource.Id.txtDescription);
			var txtDate = view.FindViewById<TextView> (Resource.Id.txtDate);
			var imgWeather = view.FindViewById<ImageView> (Resource.Id.imgWeather);
	
			txtDescription.Text = item.description;

			var dtDateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds( item.dt ).ToLocalTime();
			txtDate.Text = dtDateTime.ToLongDateString();

			ImageDownloader.AssignImageAsync(imgWeather, item.icon);

			return view;
		}

		public override int Count {
			get {
				return Items.Length;
			}
		}


		public override WeatherData this [int index] {
			get {
				return Items [index];
			}
		}

	}
}

