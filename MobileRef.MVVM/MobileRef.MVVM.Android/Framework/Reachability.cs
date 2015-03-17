using System;
using Android.App;
using Android.Net;
using Android.Content;

namespace MobileRef.MVVM.Android
{
	// THIS IS THE REACHABILITY PORTION OF THE CODE
	public enum NetworkState
	{
		Unknown,
		ConnectedWifi,
		ConnectedData,
		Disconnected
	}

	public class NetworkStatusMonitor
	{
		private static NetworkState _state;

		public NetworkStatusMonitor ()
		{
		}

		public static NetworkState State {
			get { 
				UpdateNetworkStatus ();

				return _state; 
			}
		}

		public static void UpdateNetworkStatus ()
		{
			_state = NetworkState.Unknown;

			// Retrieve the connectivity manager service
			var connectivityManager = (ConnectivityManager)
				Application.Context.GetSystemService (
					Context.ConnectivityService);

			// Check if the network is connected or connecting.
			// This means that it will be available, 
			// or become available in a few seconds.
			var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;

			if (activeNetworkInfo != null) {
				if (activeNetworkInfo.IsConnectedOrConnecting) {
					// Now that we know it's connected, determine if we're on WiFi or something else.
					_state = activeNetworkInfo.Type == ConnectivityType.Wifi ?
						NetworkState.ConnectedWifi : NetworkState.ConnectedData;
				} else {
					_state = NetworkState.Disconnected;
				}
			} else {
				_state = NetworkState.Disconnected;
			}
		}
	}

	[BroadcastReceiver ()]
	public class NetworkStatusBroadcastReceiver: BroadcastReceiver
	{

		public event EventHandler ConnectionStatusChanged;

		public override void OnReceive (Context context, Intent intent)
		{
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged (this, EventArgs.Empty);
		}
	}

	public delegate void StateChangedHandler(bool connectionState);
	public class Reachability
	{
		private static NetworkStatusBroadcastReceiver m_BroadcastReceiver;

		public static NetworkState CurrentConnectionStatus { get; set; }
		public static event StateChangedHandler StateChanged;
		private static void OnNetworkStatusChanged (object sender, EventArgs e)
		{
			var state = NetworkStatusMonitor.State;
			if (state != CurrentConnectionStatus) {
				//Status has changed
				CurrentConnectionStatus = state;
				#if DEBUG
				Console.WriteLine ("******* Status Event Fired " + state + " ************");
				#endif
				if (CurrentConnectionStatus == NetworkState.Disconnected || CurrentConnectionStatus == NetworkState.Unknown) {
					NotifyStateChange (false);
				} else {
					NotifyStateChange (true);
				}
			}

		}

		private static void NotifyStateChange(bool connectionState){
			if (StateChanged != null) {
				StateChanged (connectionState);
			}
		}

		public static void Start ()
		{
			if (m_BroadcastReceiver != null) {
				throw new InvalidOperationException (
					"Network status monitoring already active.");
			}
			// Create the broadcast receiver and bind the event handler
			// so that the app gets updates of the network connectivity status
			m_BroadcastReceiver = new NetworkStatusBroadcastReceiver ();
			m_BroadcastReceiver.ConnectionStatusChanged += OnNetworkStatusChanged;
			CurrentConnectionStatus = NetworkStatusMonitor.State;
			if (CurrentConnectionStatus == NetworkState.Disconnected || CurrentConnectionStatus == NetworkState.Unknown) {
				NotifyStateChange (false);
			} else {
				NotifyStateChange (true);
			}
			// Register the broadcast receiver
			Application.Context.RegisterReceiver (m_BroadcastReceiver, new IntentFilter (ConnectivityManager.ConnectivityAction));
		}

		public static void Stop ()
		{
			if (m_BroadcastReceiver == null) {
				throw new InvalidOperationException (
					"Network status monitoring not active.");
			}

			// Unregister the receiver so we no longer get updates.
			Application.Context.UnregisterReceiver (m_BroadcastReceiver);

			// Set the variable to nil, so that we know the receiver is no longer used.
			m_BroadcastReceiver.ConnectionStatusChanged -= OnNetworkStatusChanged;
			m_BroadcastReceiver = null;

		}
	}
}

