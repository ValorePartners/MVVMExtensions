﻿using System;
using System.Collections.Generic;
using System.Linq;
#if __ANDROID__
using Android.Runtime;
#elif __IOS__
using Foundation;
#endif

namespace MobileRef.MVVM.Shared
{
	[Preserve(AllMembers=true)]
	public class Coord
	{
		public double lon { get; set; }
		public double lat { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class Sys
	{
		public int population { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class City
	{
		public int id { get; set; }
		public string name { get; set; }
		public Coord coord { get; set; }
		public string country { get; set; }
		public int population { get; set; }
		public Sys sys { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class Temp
	{
		public double day { get; set; }
		public double min { get; set; }
		public double max { get; set; }
		public double night { get; set; }
		public double eve { get; set; }
		public double morn { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class Weather
	{
		public int id { get; set; }
		public string main { get; set; }
		public string description { get; set; }
		public string icon { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class List
	{
		public int dt { get; set; }
		public Temp temp { get; set; }
		public double pressure { get; set; }
		public int humidity { get; set; }
		public List<Weather> weather { get; set; }
		public double speed { get; set; }
		public int deg { get; set; }
		public int clouds { get; set; }
		public double? rain { get; set; }
	}
	[Preserve(AllMembers=true)]
	public class RootObject
	{
		public string cod { get; set; }
		public double message { get; set; }
		public City city { get; set; }
		public int cnt { get; set; }
		public List<List> list { get; set; }
	}

	[Preserve(AllMembers=true)]
	public class WeatherData{
		public string description { get; set; }
		public string icon { get; set; }
		public int dt { get; set; }
		public int deg { get; set; }

		public WeatherData ()
		{
			
		}
		public WeatherData (List obj)
		{
			this.dt = obj.dt;
			this.deg = obj.deg;
			var weather = obj.weather.FirstOrDefault();
			if(weather!=null){
				this.description = weather.description;
				this.icon = string.Format ("http://openweathermap.org/img/w/{0}.png", weather.icon);
			}
		}
	}
}

