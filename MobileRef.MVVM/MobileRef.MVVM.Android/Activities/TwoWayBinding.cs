using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MobileRef.MVVM.Shared;
using Android.Views;

namespace MobileRef.MVVM.Android
{
	[Activity (Label = "TwoWayBinding")]			
	public class TwoWayBinding : Activity,IHandlers
	{
		public TextView lblHello{ get; private set; }
		public EditText txtHello{ get; private set; }
		public Button btnNavigate{ get; private set;}
		public Button btnTableBinding{ get; private set; }
		public BindableHUD Progress{ get; private set;}

		public BindingViewModel VM {get;set;}

		private BindingManager<TwoWayBinding,BindingViewModel> bind;

		protected override void OnCreate (Bundle bundle)
		{
	
			base.OnCreate (bundle);
			Progress = new BindableHUD (this);
			VM = AppData.BindingVM;
			ActionBar.SetHomeButtonEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			bind = new BindingManager<TwoWayBinding, BindingViewModel> ();
			SetContentView (Resource.Layout.TwoWayBinding);

			this.CreateControls<TwoWayBinding> ();

			bind.BindProperty (() => lblHello.Text, () => VM.HelloText);
			bind.BindProperty (() => txtHello.Text, () => VM.HelloText);
			bind.BindProperty (() => Progress.Message, () => VM.LoadMessage);
			bind.BindProperty (() => Progress.Visible, () => VM.IsLoading);
			bind.BindCommand (() => btnNavigate, () => VM.TestExecute);

		}

	
		protected override void OnResume ()
		{
			bind.RegisterBindingEvents (this, VM);
			base.OnResume ();
		}
		protected override void OnPause ()
		{
			bind.UnRegisterBindingEvents (this);
			AppData.SaveBindingVM ();
			base.OnPause ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case global::Android.Resource.Id.Home:
				Finish ();
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		#region IHandlers implementation

		public void ViewEventHandler (object e, System.EventArgs args)
		{

		}

		public void ControlsHandler (object sender, System.EventArgs args)
		{
			if (sender is Button) {
				var id = ((Button)sender).Id;
				switch (id) {
				case Resource.Id.btnTableBinding:
					this.NavigateActivity<TableDisplay> (false);
					break;
				}
			}
		}

		#endregion
	}
}

