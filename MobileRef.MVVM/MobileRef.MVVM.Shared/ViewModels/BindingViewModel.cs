using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
#if __ANDROID__
using Android.Runtime;
#elif __IOS__
using Foundation;
#endif
namespace MobileRef.MVVM.Shared
{
	[Preserve(AllMembers=true)]
	public class BindingViewModel: VMBase
	{
		private int cnt = 0;
		private string helloText;
		private int age;
		public ICommand TestExecute{ get; set; }

		public int Age {
			get {
				return age;
			}
			set {
				age = value;
				Notify ("Age");
			}
		}

		public string HelloText {
			get {
				return helloText;
			}
			set {
				helloText = value;
				Notify ("HelloText");
			}
		}

		public BindingViewModel ()
		{
			TestExecute = new RelayCommand (() => {
				HelloText="Clicked " + cnt++;
				this.LongExecution();
			});
		}

		public async void LongExecution ()
		{
			try {
				LoadMessage = "Long running process...";
				IsLoading = true;
				await this.DoLongExecution ();
				IsLoading = false;
			} catch (Exception ex) {
				IsLoading = false;
			}
		}

		private async Task<bool> DoLongExecution ()
		{

			return await Task.Run (() => {
				Thread.Sleep (5000);
				return true;
			});
		}

	}
}

