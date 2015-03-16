using System;
using Foundation;
using UIKit;
using MobileRef.MVVM.Shared;
using System.Linq;
namespace MobileRef.MVVM.IOS
{
	public partial class NOSqlData : UIViewController,IHandlers
	{
		private BindableProgress Progress{ get; set; }
		private BindingManager<NOSqlData,DatabaseViewModel> bind;
		public DatabaseViewModel VM{get;set;}
		public TableSource<Person,PersonCell> Source{ get; set; }

		public NOSqlData (IntPtr handle) : base (handle)
		{
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			VM = new DatabaseViewModel ();
			bind = new BindingManager<NOSqlData, DatabaseViewModel> ();
			Source = new TableSource<Person, PersonCell> (VM.People.ToArray (), "PersonCellID"){ TableView = this.lstPeople };
			lstPeople.Source = Source;
			Progress = new BindableProgress (this.View);

			bind.BindProperty (() => txtSearch.Text, () => VM.SearchCritiera);
			bind.BindCommand (() => btnSearch, () => VM.LoadData);
			bind.BindProperty (() => Source.Items, () => VM.People);
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

		}

		public void ControlsHandler (object sender, EventArgs args)
		{

		}

		#endregion
	}
}
