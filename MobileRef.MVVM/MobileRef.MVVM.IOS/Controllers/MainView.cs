using System;
using UIKit;
using MobileRef.MVVM.Shared;
using System.Threading;

namespace MobileRef.MVVM.IOS
{
	public partial class MainView : UIViewController, IHandlers
	{
		public MainView (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}
			
		public override void ViewWillAppear (bool animated)
		{
			this.RegisterEvents ();
			base.ViewWillAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			this.UnRegisterEvents ();
			base.ViewWillDisappear (animated);
		}

		#region IHandlers implementation

		public void ViewEventHandler (object e, EventArgs args)
		{

		}

		public void ControlsHandler (object sender, EventArgs args)
		{
			if (sender == btnSimpleMessage) {
				this.ShowMessage ("Simple Message", "You clicked the button!");
				return;
			}
			if (sender == btnMultipleMessage) {
				this.ShowMessage ("Multiple", "Please select:", 
					"Apples", () => {
					Title = "Apples";
				}, "Oranges", () => {
					Title = "Oranges";
				});
				return;
			}
			if (sender == btnActionSheet) {
				this.ShowActionSheet (this.View.Frame, "Action Sheet",
					"Hello Kitty",
					new string[]{ "Apples", "Oranges", "Melons", "Pizza" },
					(selection) => {
						Title = selection;
					});
				return;
			}
			if (sender == btnInputDialog) {
				this.ShowMessage ("Enter age:", string.Empty, "Save", "In dog years", UIKeyboardType.NumberPad, (str) => {
					if (!string.IsNullOrEmpty (str)) {
						var dogYears = int.Parse (str);
						Title = string.Format ("You are {0} years old", dogYears);
					}
				});
				return;
			}
			if (sender == btnLocalNotification) {
				this.ShowLocalNotification ("Hello there", 1, 5, null);
			}
			if (sender == btnModalPopup) {
	
//				throw new NullReferenceException("test nre from ui thread.");
//		
//				ThreadPool.QueueUserWorkItem(unused =>
//					{
//						throw new NullReferenceException("test nre from back thread.");
//					});

				try {
					int x = 10;
					int y = 0;
					int z = x/y;
				} catch (Exception ex) {
					//ReportingService.PostErrorAsync (ex);
				}
			}
			if (sender == btnNavigateParams) {
				this.PushController ("TwoWayBinding", "VM", new BindingViewModel (){ HelloText = "Just a test" });
				return;
			}
			if (sender == btnNavigate) {
				this.PushController ("NOSqlData");
			}

		}

		#endregion
	}
}
