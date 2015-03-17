using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using MobileRef.MVVM.Shared;

namespace MobileRef.MVVM.Android
{
	[Application]		
	public class MainApp : Application
	{
		public MainApp(IntPtr handle, JniHandleOwnership transfer)
			: base(handle, transfer)
		{

		}

		public override void OnCreate ()
		{
			base.OnCreate ();

			//This makes sure that the control event and properties are not stripped by the linker 
			LinkerPrepare.Init (this.ApplicationContext);

			Reachability.StateChanged += (state) => {
				AppData.IsConnected = state;
			};
			Reachability.Start ();

			AppDb.Init ();
			Preload.InitDatabase ();
		}
	}
}

