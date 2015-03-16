using System;
using System.Windows.Input;

namespace MobileRef.MVVM.Shared
{

	public class BindingViewModel: VMBase
	{
		private int cnt = 0;
		private string helloText;
		public ICommand TestExecute{ get; set; }


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

	}
}

