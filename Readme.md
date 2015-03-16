MVVMExtensions Framework
===================

###Overview

There are some great MVVM frameworks out there and each have features that are incredible. As much as I would like to use something created by someone else and not do the work myself, I found that some of the frameworks had naming conventions, inheritance rules and other features (Navigation) that didn’t fit my way of coding.  Also, I didn’t have the source to see what was really going or deal with issues related to linking when Mono got grumpy.  I also didn’t have the option to extend the framework to my vision of how things should be organized and work. Again, the guys who built these frameworks did an awesome job but I had to tinker with it myself.

There are two major features to this framework that need to be called out.  First, similar to Xamarin plugins, I provided extensions to call common operations in each platform. Plugins are great but I didn’t want to download an individual library for each feature and I wanted the source code handy so I could change something as needed.  Also, the source code provides a reference implementation if you want to do it yourself. Another extension in the framework also connects and disconnects events in bulk to ensure that classes can be disposed properly as well as instantiating controls in Android rather than invoking FindByID numerous times.

Second, a basic MVVM implementation is provided allowing common controls to bind to view model properties including ICommand.  Defined in the framework is also a simple implementation of Android’s base adapter and iOS’ table source to facilitate binding as well as the view holder pattern in Android.  These patterns can be applied to more complex table structures as needed.

What is really important is that nothing is forced upon you.  You can choose to use only the features you like and nothing is implemented by inheritance.  Everything is either an extension or an interface.

###Code Example

This example displays how Android buttons are defined and the extension method CreateControls finds and instantiates them.  The onStart and OnStop methods are where we can register and unregister their events.  Download the entire project to view the entire implementation.

	public class MainActivity : Activity, IHandlers
	{
		public Button btnSimpleMessage{ get; private set; }

		public Button btnMultipleMessage{ get; private set; }

		public Button btnInputDialog{ get; private set; }

		public Button btnNavigateParams{ get; private set; }

		public Button btnNavigate{ get; private set; }

		public Button btnProgress{ get; private set; }

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
			SetContentView (Resource.Layout.Main);
			this.CreateControls<MainActivity> ();
		}
		public void ViewEventHandler (object e, EventArgs args)
		{

		}

		public void ControlsHandler (object sender, EventArgs args)
		{
	
		}
	}


This iOS example demonstrates data-binding using lambda expressions to hookup an input field and label to a view model’s property called ‘HelloText’.  A button is also connected to an ICommand property while a progress indicator displays loading information based on a text and bool property. If a button is not data bound to the view model it will be register as an event to be fired to the ControlHandler method.  The ViewWillAppear and Disappear methods register/bind and unregister/unbind the events for the entire controller.

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
