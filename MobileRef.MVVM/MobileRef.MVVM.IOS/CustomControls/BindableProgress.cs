using System;
using MBProgressHUD;
using UIKit;

namespace MobileRef.MVVM.IOS
{
	public class BindableProgress
	{
		private MTMBProgressHUD hud;
		public BindableProgress (UIView view)
		{
			hud = new MTMBProgressHUD (view) {
				LabelText = "Waiting...",
				RemoveFromSuperViewOnHide = false
			};

			view.AddSubview (hud);
		}

		public string LoadingMessage{
			get{
				return hud.LabelText;
			}
			set{
				hud.LabelText = value;
			}
		}
		public bool IsVisible{
			get{
				if (hud != null)
					return true;
				else
					return false;
			}
			set{
				if (value) {
					hud.Show (animated: true);
				} else {
					hud.Hide (animated: true, delay: 0);
				}
			}
		}
	}
}

