using System;
using System.Windows.Input;

namespace MobileRef.MVVM.Shared
{

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

	}
}

