using System;
using System.ComponentModel;
using System.Windows.Input;

namespace  MobileRef.MVVM.Shared
{
	public class RelayCommand : ICommand
	{
		private Action _execute;

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return true;
		}

		public RelayCommand (Action execute)
		{
			_execute = execute;
		}

		public void Execute (object parameter)
		{
			_execute ();
		}
	}

	public class VMBase: INotifyPropertyChanged,IDisposable
	{
		private bool isLoading;
		private string loadingMessage;
		private bool disposed = false;

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsLoading {
			get {
				return isLoading;
			}
			set {
				isLoading = value;
				Notify ("IsLoading");
			}
		}

		public string LoadMessage {
			get {
				return loadingMessage;
			}
			set {
				loadingMessage = value;
				Notify ("LoadMessage");
			}
		}

		public VMBase ()
		{

		}

		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose ()
		{ 
			Dispose (true);
			GC.SuppressFinalize (this);           
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here. 
				//
			}

			// Free any unmanaged objects here. 
			//
			disposed = true;
		}

		~VMBase ()
		{
			Dispose (false);
		}

		protected void Notify (string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
		}

		protected async void LogException (Exception ex, string methodName)
		{
			#if DEBUG
			Console.WriteLine ("*************  {0} - {1}  **************", this.GetType ().Name, methodName);
			Console.WriteLine (ex.Message);
			if (ex.InnerException != null)
				Console.WriteLine (ex.InnerException.Message);
			Console.WriteLine ("      -----Stack-----");
			Console.WriteLine (ex.StackTrace);
			Console.WriteLine ("***************************");
			#endif

			ReportingService.PostErrorAsync (ex, 1, SeverityType.Handled, this.GetType ().Name, methodName);
		}

	}

}
