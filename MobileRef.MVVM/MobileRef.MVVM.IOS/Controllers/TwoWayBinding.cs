using System;
using UIKit;
using MobileRef.MVVM.Shared;


namespace MobileRef.MVVM.IOS
{
	public partial class TwoWayBinding : UIViewController,IHandlers
	{
		private BindableProgress Progress{ get; set; }
		public BindingViewModel VM {get;set;}
		private BindingManager<TwoWayBinding,BindingViewModel> bind;
		public TwoWayBinding (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			bind = new BindingManager<TwoWayBinding, BindingViewModel> ();
			Progress = new BindableProgress (this.View);

			bind.BindProperty (() => lblHello.Text, () => VM.HelloText);
			bind.BindProperty (() => txtHello.Text, () => VM.HelloText);

			bind.BindProperty (() => txtAlt.Text, () => VM.Age);
			bind.BindProperty (() => lblAlt.Text, () => VM.Age);

			//Age
			bind.BindCommand (() => btnNavigate, () => VM.TestExecute);
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

		public void ViewEventHandler (object e, EventArgs args)
		{

		}

		public void ControlsHandler (object sender, EventArgs args)
		{
			if (sender == btnTableBinding) {
				this.PushController ("TableDisplay");
			}
		}
	}
}
