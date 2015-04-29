using System;
using UIKit;
using CoreGraphics;
using System.Threading.Tasks;
using Foundation;

namespace $rootnamespace$
{
	public static class VPControlExtensions
	{
		public static CGRect CopyFrame (this UIView view, nfloat? x = null, nfloat? y = null)
		{
			if (x.HasValue && !y.HasValue) {
				return new CGRect (x.Value, view.Frame.Y, view.Frame.Width, view.Frame.Height);
			} else if (!x.HasValue && y.HasValue) {
				return new CGRect (view.Frame.X, y.Value, view.Frame.Width, view.Frame.Height);
			} else if (x.HasValue && y.HasValue) {
				return new CGRect (x.Value, y.Value, view.Frame.Width, view.Frame.Height);
			} else {
				return new CGRect (view.Frame.X, view.Frame.Y, view.Frame.Width, view.Frame.Height);
			}
		}

		public static UIImage Resize (this UIImage img, float newHeight)
		{
			var percent = newHeight / img.Size.Height;
			var newWidth = percent * img.Size.Width;
			var newSize = new CGSize (newWidth, newHeight);
			UIGraphics.BeginImageContextWithOptions (newSize, false, UIScreen.MainScreen.Scale);
			img.Draw (new CGRect (0, 0, newSize.Width, newSize.Height));
			img = UIGraphics.GetImageFromCurrentImageContext ();
			return img;
		}

		public static UIImage ChangeColor (this UIImage image, UIColor color)
		{
			var rect = new  CGRect (0, 0, image.Size.Width, image.Size.Height);
			UIGraphics.BeginImageContext (rect.Size);
			var ctx = UIGraphics.GetCurrentContext ();
			ctx.ClipToMask (rect, image.CGImage);
			ctx.SetFillColor (color.CGColor);
			ctx.FillRect (rect);
			var img = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return UIImage.FromImage (img.CGImage, 1.0f, UIImageOrientation.DownMirrored);
		}

		public static UIColor HexToColor (this int hexValue)
		{
			return UIColor.FromRGB (
				(((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
				(((float)(hexValue & 0xFF)) / 255.0f)
			);
		}

		public static void ApplyDropShadow (this UIView view)
		{
			view.Layer.ShadowColor = (UIColor.DarkGray).CGColor;
			view.Layer.ShadowOffset = new CGSize (0, 1);
			view.Layer.ShadowOpacity = 1;
			view.Layer.ShadowRadius = 1;
		}

		/// <summary>
		/// Load image async when instead of a UITableView Cell
		/// </summary>
		/// <param name="imgView">Image view.</param>
		/// <param name="uri">URI.</param>
		/// <param name="view">View.</param>
		/// <param name="path">Path.</param>
		public static async void LoadFromURL (this UIImageView imgView, string uri, UITableView view, NSIndexPath path)
		{
			try {
				await Task.Factory.StartNew (() => {
					using (var url = new NSUrl (uri)) {
						using (var data = NSData.FromUrl (url)) {
							imgView.InvokeOnMainThread (() => {
								imgView.Image = UIImage.LoadFromData (data);
								view.ReloadRows (new NSIndexPath[]{ path }, UITableViewRowAnimation.None);
							});
						}
					}
				});
			} catch (Exception) {
				//do nothing
			}
		}

		public static async void LoadFromURL (this UIImageView imgView, string uri)
		{
			try {
				await Task.Factory.StartNew (() => {
					using (var url = new NSUrl (uri)) {
						using (var data = NSData.FromUrl (url)) {
							imgView.InvokeOnMainThread (() => {
								imgView.Image = UIImage.LoadFromData (data);
							});
						}
					}
				});
			} catch (Exception) {
				//do nothing
			}
		}

		public static void AddPadding (this UITextField control)
		{
			var padding = new UIView (new CGRect (0, 0, 10, 20));
			control.LeftView = padding;
			control.LeftViewMode = UITextFieldViewMode.Always;
		}

	}
}

