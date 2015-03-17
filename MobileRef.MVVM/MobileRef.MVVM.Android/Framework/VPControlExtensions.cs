using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Android.App;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Net;

namespace MobileRef.MVVM.Android
{

	[Preserve (AllMembers = true)]
	public static class VPControlExtensions
	{
		public static void LoadUrl (this ImageView img, Activity activity, string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient ()) {
				var imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}
			img.SetImageBitmap (imageBitmap);
			Runnable run = new Runnable (img.Invalidate);
			activity.RunOnUiThread (run);
		}
		public static void LoadUrlBySynchronizationContext (this ImageView img, string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient ()) {
				var imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}
			img.SetImageBitmap (imageBitmap);
			Application.SynchronizationContext.Post (_ => {
				img.SetImageBitmap (imageBitmap);
				img.Invalidate();
			}, null);
		}

		public static void ResetHeight (this ListView listView)
		{
			if (listView == null || listView.Adapter == null) {
				return;
			}
			try {
				var row = 0;
				int totalHeight = listView.PaddingTop + listView.PaddingBottom;
				for (int i = 0; i < listView.Count; i++) {
					View listItem = listView.Adapter.GetView (i, null, listView);
					var p = listItem.LayoutParameters;
					var v = ((ViewGroup)listItem);
					if (listItem.GetType () == typeof(ViewGroup)) {
						listItem.LayoutParameters = new LinearLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
					}
					listItem.Measure (0, 0);
					row = listItem.MeasuredHeight;
				}

				listView.LayoutParameters.Height = (row * listView.Count) + (listView.DividerHeight * (listView.Count - 1));
				listView.RequestLayout ();
			} catch (System.Exception ex) {
				#if DEBUG
				Console.WriteLine (ex.Message);
				#endif
			}
		}
	}

	public class ImageDownloader
	{
		private static readonly Dictionary<string, Bitmap> UrlToImageMap;

		static ImageDownloader()
		{
			UrlToImageMap = new Dictionary<string, Bitmap>();
		}

		public static void AssignImageAsync(ImageView image, string url)
		{
			var info = new ImageDownloadInfo
			{
				ImageView = image,
				ImageUrl = url,
			};

			ThreadPool.QueueUserWorkItem(DownloadImage, info);
		}

		private static void DownloadImage(object state)
		{
			try
			{
				var info = (ImageDownloadInfo)state;
				Bitmap bitmap;
				lock (UrlToImageMap)
				{
					if (UrlToImageMap.ContainsKey(info.ImageUrl))
					{
						bitmap = UrlToImageMap[info.ImageUrl];
					}
					else
					{
						var imageUrl = new URL(info.ImageUrl);
						bitmap = BitmapFactory.DecodeStream(imageUrl.OpenStream());
						UrlToImageMap.Add(info.ImageUrl, bitmap);
					}
				}
				Application.SynchronizationContext.Post (_ => {
					info.ImageView.SetImageBitmap(bitmap);
				}, null);
			}
			catch (System.Exception)
			{
				// Log error, etc
			}
		}

		private class ImageDownloadInfo
		{

			public string ImageUrl
			{
				get;
				set;
			}

			public ImageView ImageView
			{
				get;
				set;
			}
		}
	}

}
