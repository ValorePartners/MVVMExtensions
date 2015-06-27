using System;
using System.Threading.Tasks;

namespace MobileRef.MVVM.Shared
{
	public enum DeploymentState
	{
		Dev,
		QA,
		Prod
	}
	public enum SeverityType
	{
		Crash,
		Handled
	}
	public class MobileError
	{
		public int Id { get; set; }
		public int AppId { get; set; }
		public string ErrorType { get; set; }
		public string ErrorMessage { get; set; }
		public string ClassName { get; set; }
		public string MethodName { get; set; }
		public string ErrorStack { get; set; }
		public string ErrorInnerMessage { get; set; }
		public long ErrrorOccurred { get; set; }
		public string Platform { get; set; }
		public string AppVersion { get; set; }
		public DeploymentState DeploymentState { get; set; }
		public SeverityType Severity { get; set; }

		public MobileError (Exception ex, int appId, SeverityType severity, string className, string methodName)
		{
			this.AppId = appId;
			this.ErrorMessage = ex.Message;
			this.ErrorStack = ex.StackTrace;
			this.ErrrorOccurred = DateTime.UtcNow.Ticks;
			this.AppVersion = AppData.AppVersion;
			this.Platform = AppData.Device.ToString ();
			this.ErrorInnerMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
			this.ErrorType = ex.GetType ().Name;
			this.DeploymentState = AppData.DeployState;
			this.Severity = severity;
			this.ClassName = className;
			this.MethodName = methodName;
		}
	}
	public class Tracking
	{
		public int Id { get; set; }
		public int AppId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string TrackingPointP { get; set; }
		public long TimeStamp { get; set; }
		public string Platform { get; set; }
		public string AppVersion { get; set; }

		public Tracking (int appId, int userId, string track, string platform, string version)
		{
			this.AppId = appId;
			this.UserId = userId;
			this.TrackingPointP = track;
			this.TimeStamp =  DateTime.UtcNow.Ticks;
			this.Platform = platform;
			this.AppVersion = version;
		}

	}
	public class PushRegistrations
	{
		public int Id { get; set; }
		public int AppId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string RegistrationId { get; set; }
		public long TimeRegistered { get; set; }
		public string Platform { get; set; }
		public string AppVersion { get; set; }

		public PushRegistrations (int appId, int userId, string RegistrationId, string platform, string version)
		{
			this.AppId = appId;
			this.UserId = userId;
			this.RegistrationId = RegistrationId;
			this.TimeRegistered =  DateTime.UtcNow.Ticks;
			this.Platform = platform;
			this.AppVersion = version;
		}
	}


	public class ReportingService
	{
		private static RestService service;
		private static bool isPosting;

		public static void PostError (Exception ex, int appId, SeverityType severity, string className, string methodName)
		{
			if (service == null)
				service = new RestService ();
			if (!isPosting) {
				isPosting = true;
				service.PostNoReturn (AppData.ReportExceptionUrl, new MobileError (ex, appId, severity, className, methodName));
				isPosting = false;
			}
		}

		public static async void PostErrorAsync (Exception ex, int appId, SeverityType severity, string className, string methodName)
		{
			if (AppData.DeployState != DeploymentState.Prod) {
				await Task.Run (() => {
					PostError (ex, appId, severity, className, methodName);
				});
			} else {
				PersistException (ex);
			}
		}

		private static void PersistException (Exception ex)
		{

		}

		public static async void PostLoggedExceptions ()
		{

		}
	}
	public class TrackingService
	{
		private static RestService service;
		private static bool isPosting;

		public static void LogAnalytics (int appId, int userId, string track, string platform, string version)
		{
			if (service == null)
				service = new RestService ();
			if (!isPosting) {
				isPosting = true;
				service.PostNoReturn (AppData.ReportAnalyticsUrl, new Tracking (appId, userId, track, platform,version));
				isPosting = false;
			}
		}

		public static async void LogAnalyticsAsync (int appId, int userId, string track, string platform, string version)
		{

			await Task.Run (() => {
				LogAnalytics (appId, userId, track, platform, version);
			});
	
		}
	}
	public class NotificationRegistrationService
	{
		private static RestService service;
		private static bool isPosting;

		public static void LogAnalytics (int appId, int userId, string RegistrationId, string platform, string version)
		{
			if (service == null)
				service = new RestService ();
			if (!isPosting) {
				isPosting = true;
				service.PostNoReturn (AppData.ReportAnalyticsUrl, new Tracking (appId, userId, RegistrationId, platform,version));
				isPosting = false;
			}
		}

		public static async void LogAnalyticsAsync (int appId, int userId, string RegistrationId, string platform, string version)
		{

			await Task.Run (() => {
				LogAnalytics (appId, userId, RegistrationId, platform, version);
			});

		}
	}
}

