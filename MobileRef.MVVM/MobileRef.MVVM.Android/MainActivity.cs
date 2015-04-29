using System;
using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Widget;
using MobileRef.MVVM.Shared;

namespace MobileRef.MVVM.Android
{
	[Activity (Label = "MobileRef.MVVM.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IHandlers
	{
		private BindableHUD progress;

		public Button btnSimpleMessage{ get; private set; }

		public Button btnMultipleMessage{ get; private set; }

		public Button btnInputDialog{ get; private set; }

		public Button btnNavigateParams{ get; private set; }

		public Button btnNavigate{ get; private set; }

		public Button btnProgress{ get; private set; }

		public VMBase VM { get; set; }



		protected override void OnStart ()
		{
			this.RegisterEvents ();
			base.OnStart ();
		}

		protected override void OnStop ()
		{
			this.UnRegisterEvents ();
			base.OnStop ();
		}
			
		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			VM = new VMBase ();
			SetContentView (Resource.Layout.Main);
			progress = new BindableHUD (this);
			this.CreateControls<MainActivity> ();
		}
			
		#region IHandlers implementation

		public void ViewEventHandler (object e, EventArgs args)
		{
			if (args is PropertyChangedEventArgs) {
				var propName = ((PropertyChangedEventArgs)args).PropertyName;
				switch (propName) {
				case "LoadingMessage":
					progress.Message = VM.LoadMessage;
					break;
				case "IsLoading":
					progress.Visible = VM.IsLoading;
					break;
				}
			}
		}

		public void ControlsHandler (object sender, EventArgs args)
		{
			if (sender is Button) {
				var id = ((Button)sender).Id;
				switch (id) {
				case Resource.Id.btnInputDialog:
					var ageInput = new EditText (this){ InputType = global::Android.Text.InputTypes.ClassNumber };
					this.ShowMessage ("Enter your age",
						"Save", () => {
						if (!string.IsNullOrEmpty (ageInput.Text) && ageInput.Text.Length <= 6) {
							var age = int.Parse (ageInput.Text);
							this.ShowMessage ("Input values:", "You entered " + age);
						}
					},
						ageInput);
					break;
				case Resource.Id.btnMultipleMessage:
					this.ShowMessage ("Multiple", "Please select:", 
						"Apples", () => {
						Title = "Apples";
					}, "Oranges", () => {
						Title = "Oranges";
					});
					return;
					break;
				case Resource.Id.btnNavigate:
					this.NavigateActivity<NOSqlData> (false);
					break;
				case Resource.Id.btnNavigateParams:
					this.NavigateActivity<TwoWayBinding> ("VM", new BindingViewModel(){ HelloText="Just a test"}, false);
					break;
				case Resource.Id.btnSimpleMessage:
					this.ShowMessage ("Simple Message", "You clicked the button!");
					break;
				case Resource.Id.btnProgress:
					//VM.LongExecution ();
					break;
				}

			}
		}

		#endregion

		protected override void OnDestroy ()
		{
			((MainApp)this.Application).ApplicationClosing ();
			base.OnDestroy ();
		}

	}
}


