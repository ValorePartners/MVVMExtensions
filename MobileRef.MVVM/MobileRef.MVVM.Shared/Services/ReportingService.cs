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

		public string ErrorType { get; set; }

		public string ErrorMessage { get; set; }

		public string ClassName{ get; set; }

		public string MethodName{ get; set; }

		public string ErrorStack { get; set; }

		public string ErrorInnerMessage { get; set; }

		public long ErrrorOccurred { get; set; }

		public string Platform { get; set; }

		public string AppVersion { get; set; }

		public DeploymentState DeploymentState{ get; set; }

		public SeverityType Severity { get; set; }

		public MobileError ()
		{
			
		}

		public MobileError (Exception ex, SeverityType severity, string className, string methodName)
		{
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

	public class ReportingService
	{
		private static RestService service;
		private static bool isPosting;

		public static void PostError (Exception ex, SeverityType severity, string className, string methodName)
		{
			if (service == null)
				service = new RestService ();
			if (!isPosting) {
				isPosting = true;
				service.PostNoReturn (AppData.ReportExceptionUrl, new MobileError (ex, severity, className, methodName));
				isPosting = false;
			}
		}

		public static async void PostErrorAsync (Exception ex, SeverityType severity, string className, string methodName)
		{
			if (AppData.DeployState != DeploymentState.Prod) {
				await Task.Run (() => {
					PostError (ex, severity, className, methodName);
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
}

