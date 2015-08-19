using System.Collections.Generic;

namespace MobileRef.MVVM.Shared
{

	public enum DeviceType{
		IOS,
		ANDROID,
		WINPHONE
	}
	public class AppData
	{
		#region Application constants
		public const string ReportExceptionUrl = "http://mobilemgttemplate.azurewebsites.net/api/Reporting/LogException";
		public const string ReportAnalyticsUrl = "http://mobilemgttemplate.azurewebsites.net/api/Analytics/LogTracking";
		public const string RegisterDeviceUrl = "http://mobilemgttemplate.azurewebsites.net/api/PushNotification/AddRegistration";
		public const string AppVersion = "1.0";
		#endregion

		#region Application Level Properties
		public static bool IsConnected{ get; set; }
		public static DeviceType Device{ get; set; }

		public static DeploymentState DeployState { 
			get { 
				return DeploymentState.Dev;
			}
		}
		#endregion


		private static LocalStorageService storage;
		private static WeatherViewModel weatherVM;
		private static BindingViewModel bindingVM;
		private static DatabaseViewModel databaseVM;

		public static LocalStorageService Storage {
			get {
				if (storage == null)
					storage = new LocalStorageService ();
				return storage;
			}
		}

		public static DatabaseViewModel DatabaseVM {
			get {
				if (databaseVM == null) {
					databaseVM = Storage.GetIsolatedStorage<DatabaseViewModel> ("DatabaseViewModel");
					if (databaseVM == null)
						databaseVM = new DatabaseViewModel ();
				}
				return databaseVM;
			}
			set {
				databaseVM = value;
			}
		}

		public static BindingViewModel BindingVM {
			get {
				if (bindingVM == null) {
					bindingVM = Storage.GetIsolatedStorage<BindingViewModel> ("BindingViewModel");
					if (bindingVM == null)
						bindingVM = new BindingViewModel ();
				}
				return bindingVM;
			}
			set {
				bindingVM = value;
			}
		}

		public static WeatherViewModel WeatherVM {
			get {
				if (weatherVM == null) {
					weatherVM = Storage.GetIsolatedStorage<WeatherViewModel> ("WeatherViewModel");
					if (weatherVM == null)
						weatherVM = new WeatherViewModel ();
				}
				return weatherVM;
			}
			set {
				weatherVM = value;
			}
		}

		public static async void SaveDatabaseVM ()
		{
			await Storage.SaveIsolatedStorageAsync<DatabaseViewModel> ("DatabaseViewModel", databaseVM);
		}

		public static async void SaveBindingVM ()
		{
			await Storage.SaveIsolatedStorageAsync<BindingViewModel> ("BindingViewModel", bindingVM);
		}

		public static async void SaveWeatherVM ()
		{
			await Storage.SaveIsolatedStorageAsync<WeatherViewModel> ("WeatherViewModel", weatherVM);
		}

		public static void ClearStateManagement ()
		{
			Storage.DeleteIsolatedStorage ("DatabaseViewModel");
			Storage.DeleteIsolatedStorage ("BindingViewModel");
			Storage.DeleteIsolatedStorage ("WeatherViewModel");
		}


	}

	public class Preload
	{
		public static async void InitDatabase ()
		{

			var respository = new PersonRespository ();

			var currentCollection = await respository.GetAll ();
			if (currentCollection != null && currentCollection.Count > 0)
				return;

			var collection = new List<Person> ();
			collection.Add (new Person () {
				FirstName = "Bill",
				LastName = "Roberts",
				UserName = "bRoberts",
				Password = "8987",
				Addresses = new List<Contact> () { new Contact () {
						Address = "123 E. BlackPearl Lane",
						City = "Tortuga",
						State = "Virgin Is",
						Zip = 90210
					}
				}
			});
			collection.Add (new Person () {
				FirstName = "Jack",
				LastName = "Hamilton",
				UserName = "jHamilton",
				Password = "4532"
			});
			collection.Add (new Person (){ FirstName = "Sam", LastName = "Samson", UserName = "sSamson", Password = "644" });
			collection.Add (new Person (){ FirstName = "Fred", LastName = "Goodman", UserName = "rGoodman", Password = "643" });
			collection.Add (new Person (){ FirstName = "Rhonda", LastName = "Jules", UserName = "rJules", Password = "978" });
			collection.Add (new Person () {
				FirstName = "Valerie",
				LastName = "Williamson",
				UserName = "vWilliamson",
				Password = "111"
			});
			collection.Add (new Person (){ FirstName = "Edgar", LastName = "Madsen", UserName = "eMadsen", Password = "222" });
			collection.Add (new Person (){ FirstName = "Jasper", LastName = "Taylor", UserName = "jTaylor", Password = "333" });
			collection.Add (new Person (){ FirstName = "William", LastName = "Hobson", UserName = "wHobson", Password = "444" });
			collection.Add (new Person (){ FirstName = "Quincy", LastName = "Calvin", UserName = "qCalvin", Password = "555" });
			collection.Add (new Person (){ FirstName = "Dorris", LastName = "Brown", UserName = "dBrown", Password = "666" });
			collection.Add (new Person (){ FirstName = "Brad", LastName = "Red", UserName = "bRed", Password = "777" });

			var result = await respository.SaveCollection (collection);
		}
	}
}

