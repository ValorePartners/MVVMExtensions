using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileRef.MVVM.Shared
{
	public class WeatherServices
	{
		public RestService Http{ get; set; }
		public WeatherServices ()
		{
			Http = new RestService ();
		}

		public async Task<List<WeatherData>> GetWeatherDataForLocation(string location){
			var collection = new List<WeatherData> ();
			var url = string.Format ("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&mode=json&units=metric&cnt=7", location);
			var data = await Http.GetAsync<RootObject> (url);
			if (data != null) {
				foreach (var obj in data.list) {
					var wd = new WeatherData (obj);
					collection.Add (wd);
				}
			}
			return collection;
		}

	
	}
}

