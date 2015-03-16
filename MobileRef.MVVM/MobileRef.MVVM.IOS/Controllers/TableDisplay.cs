using System;
using Foundation;
using UIKit;
using MobileRef.MVVM.Shared;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;


namespace MobileRef.MVVM.IOS
{
	public partial class TableDisplay : UITableViewController,IHandlers
	{
		private BindableProgress Progress{ get; set; }
		public WeatherViewModel VM{ get; set; }

		private BindingManager<TableDisplay,WeatherViewModel> bind;

		//Two ways to create and use a UITableViewSource
		//public TableSource<WeatherData,WeatherCell> Source{ get; set; }
		public CustomSource Source { get; set; }


		public TableDisplay (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			VM = new WeatherViewModel ();
			bind = new BindingManager<TableDisplay, WeatherViewModel> ();
			Progress = new BindableProgress (this.View);

			//Two ways to create and use a UITableViewSource
			//Source = new TableSource<WeatherData, WeatherCell> (VM.WeatherCollection.ToArray (), "WeatherCell"){ TableView = TableView };
			Source = new CustomSource (VM.WeatherCollection.ToArray ()){ TableView = this.TableView };

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (UIBarButtonSystemItem.Action, (sender, args) => {
					this.ShowMessage ("Enter search criteria", string.Empty, "Search", "Weather location", UIKeyboardType.Default, (str) => {
						if (!string.IsNullOrEmpty (str)) {
							VM.SearchLocation = str;
							VM.PerformSearch.Execute (null);
						}
					});
				})
				, true);

			TableView.Source = Source;
			bind.BindProperty (() => Source.Items, () => VM.WeatherCollection);
			bind.BindProperty (() => Source.SelectedItem, () => VM.SelectedWeather);
			bind.BindProperty (() => Progress.IsVisible, () => VM.IsLoading);
			bind.BindProperty (() => Progress.LoadingMessage, () => VM.LoadMessage);
		}

		public override void ViewWillAppear (bool animated)
		{
			bind.RegisterBindingEvents (this, VM);
			base.ViewWillAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			bind.UnRegisterBindingEvents (this);
			base.ViewWillDisappear (animated);
		}

		#region IHandlers implementation

		public void ViewEventHandler (object e, EventArgs args)
		{
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				switch (propName) {
				case "SelectedItem":
					this.ShowMessage ("Selected", VM.SelectedWeather.description);
					break;
				}
			}
		}

		public void ControlsHandler (object sender, EventArgs args)
		{
		
		}

		#endregion
	}


	//Explicit way to create a UITableViewSource. Databinding must use the IBaseSource
	public class CustomSource:UITableViewSource,IBaseSource
	{
		private string cellId = "PersonCellId";
		public event EventHandler RowSelectedEvent;

		public WeatherData[] Items{ get; set; }

		public UITableView TableView { get; set; }

		public object SelectedItem { get; set; }

		public nint Tag { get; set; }

		public CustomSource (WeatherData[] items)
		{
			this.Items = items;
		}

		public void UpdateCollection (System.Collections.ICollection collection)
		{
			var list = new List<WeatherData> ();
			foreach (var obj in collection) {
				list.Add ((WeatherData)obj);
			}
			Items = list.ToArray ();
			TableView.ReloadData ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			var cell = tableView.DequeueReusableCell (cellId);
			if (cell == null) {
				cell = WeatherCell.Create ();
			}
			((WeatherCell)cell).Bind (Items [indexPath.Row]);
			return cell;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Items.Length;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this.SelectedItem = Items [indexPath.Row];
			if (RowSelectedEvent != null)
				RowSelectedEvent (this, null);
		}

	}
}
