using System;
using AndroidHUD;
using Android.Content;

namespace MobileRef.MVVM.Android
{
	public class BindableHUD
	{
		private string message;
		private bool isVisible;
		private Context ctx;
		public BindableHUD (Context ctx)
		{
			message = "Loading...";
			this.ctx = ctx;

		}
		public string Message { 
			get{ return message; }
			set{ message = value; }
		}


		public bool Visible{
			get{
				return isVisible;
			}
			set{
				isVisible = value;
				if(value)
					AndHUD.Shared.Show (ctx, Message, (int)MaskType.Clear);
				else
					AndHUD.Shared.Dismiss(ctx);
				//AndHUD.Shared.Show(myActivity, "Status Message", MaskType.Clear);
			}
		}
	}
}

