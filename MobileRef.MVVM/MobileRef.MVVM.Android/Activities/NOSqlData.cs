using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileRef.MVVM.Shared;

namespace MobileRef.MVVM.Android
{
	[Activity (Label = "NOSqlData")]			
	public class NOSqlData : Activity,IHandlers
	{
		public EditText txtSearch{ get; private set; }
		public Button btnSearch{ get; private set; }
		public ListView lstPeople{ get; private set; }
		public BindableHUD Progress{ get; private set;}
		public DatabaseViewModel VM{get;set;}
		public HolderAdapter<Person,PeopleViewHolder> Adapter{ get; set;}
		public BindingManager<NOSqlData,DatabaseViewModel> bind;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Progress = new BindableHUD (this);
			VM = AppData.DatabaseVM;
			bind = new BindingManager<NOSqlData, DatabaseViewModel> ();
			SetContentView (Resource.Layout.NOSqlData);
			this.CreateControls<NOSqlData> ();

			bind.BindProperty (() => txtSearch.Text, () => VM.SearchCritiera);
			bind.BindProperty (() => Progress.Message, () => VM.LoadMessage);
			bind.BindProperty (() => Progress.Visible, () => VM.IsLoading);
			bind.BindCommand (() => btnSearch, () => VM.LoadData);

			Adapter = new HolderAdapter<Person, PeopleViewHolder> (this.LayoutInflater, VM.People.ToArray ());
			bind.BindProperty (() => Adapter.Items, () => VM.People);
			lstPeople.Adapter = Adapter;
		}

		protected override void OnResume ()
		{
			bind.RegisterBindingEvents (this, VM);
			base.OnResume ();
		}
		protected override void OnPause ()
		{
			bind.UnRegisterBindingEvents (this);
			AppData.SaveDatabaseVM ();
			base.OnPause ();
		}

		#region IHandlers implementation
		public void ViewEventHandler (object e, System.EventArgs args)
		{

		}
		public void ControlsHandler (object sender, EventArgs args)
		{

		}
		#endregion

	}

	public class PeopleViewHolder:ViewHolder<Person>{

		private TextView Text1{ get; set;}
		public override void Init (View view)
		{
			Text1 = view.FindViewById<TextView>(global::Android.Resource.Id.Text1);
		}

		public override void Populate (Person item)
		{
			Text1.Text = item.FirstName + " " + item.LastName;
		}

		public override int CellResourceId {
			get {
				return global::Android.Resource.Layout.SimpleListItem1;
			}
		}
	}
}

