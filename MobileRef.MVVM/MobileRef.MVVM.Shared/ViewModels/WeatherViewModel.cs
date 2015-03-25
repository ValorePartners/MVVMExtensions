using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
#if __ANDROID__
using Android.Runtime;
#elif __IOS__
using Foundation;
#endif
namespace MobileRef.MVVM.Shared
{
	[Preserve(AllMembers=true)]
	public class WeatherViewModel:VMBase
	{
		private string searchLocation;
		private ObservableCollection<WeatherData> weatherCollection;
		private WeatherData selectedWeather;
		private WeatherServices weatherService;
		private Exception exceptionOccurred;

		public Exception ExceptionOccurred {
			get {
				return exceptionOccurred;
			}
			set {
				exceptionOccurred = value;
				Notify ("ExceptionOccurred");
			}
		}

		public WeatherData SelectedWeather {
			get {
				return selectedWeather;
			}
			set {
				selectedWeather = value;
				Notify ("SelectedWeather");
			}
		}

		public ICommand PerformSearch { get; set; }

		public string SearchLocation {
			get {
				return searchLocation;
			}
			set {
				searchLocation = value;
				Notify ("SearchLocation");
			}
		}

		public ObservableCollection<WeatherData> WeatherCollection {
			get {
				return weatherCollection;
			}
			set {
				weatherCollection = value;
				Notify ("WeatherCollection");
			}
		}


		public WeatherViewModel ()
		{
			WeatherCollection = new ObservableCollection<WeatherData> ();
			weatherService = new WeatherServices ();
			PerformSearch = new RelayCommand (async() => {
				if (!string.IsNullOrEmpty (SearchLocation)) {
					this.LoadMessage = "Searching weather information...";
					this.IsLoading = true;
					var data = await weatherService.GetWeatherDataForLocation (SearchLocation);
		
					WeatherCollection =data.ToObservable();

					this.IsLoading = false;
				}
			});
			weatherService.Http.ExceptionOccured += ErrorHandler;
		
		}

		protected override void Dispose (bool disposing)
		{
			weatherService.Http.ExceptionOccured -= ErrorHandler;
			base.Dispose (disposing);

		}

		private void ErrorHandler (Exception ex)
		{
			ExceptionOccurred = ex;
		}

	}
}

