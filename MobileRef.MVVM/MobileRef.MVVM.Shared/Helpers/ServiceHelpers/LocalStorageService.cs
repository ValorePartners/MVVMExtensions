using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace  MobileRef.MVVM.Shared
{
	public class LocalStorageService
	{
		public Task<T> GetIsolatedStorageAsync<T> (string contentName) where T : new()
		{
			Task<T> task = new Task<T> (() => {
				using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
					if (isoStorage.FileExists (contentName)) {
						var s = isoStorage.OpenFile (contentName, FileMode.OpenOrCreate);
						var sr = new StreamReader (s);
						var content = sr.ReadToEnd ();
						sr.Close ();
						return JsonConvert.DeserializeObject<T> (content);
					} else {
						return new T ();
					}
				}

			});
			task.Start ();
			return task;

		}

		public T GetIsolatedStorage<T> (string contentName) where T : class
		{
			using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
				if (isoStorage.FileExists (contentName)) {
					var s = isoStorage.OpenFile (contentName, FileMode.OpenOrCreate);
					var sr = new StreamReader (s);
					var content = sr.ReadToEnd ();
					sr.Close ();
					return JsonConvert.DeserializeObject<T> (content);
				} else {
					return null;
				}
			}

		}

		public void DeleteIsolatedStorage (string contentName)
		{
			using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
				if (isoStorage.FileExists (contentName)) {
					isoStorage.DeleteFile (contentName);
				}

			}
		}

		public Task<int> SaveIsolatedStorageAsync<T> (string contentName, object obj)
		{
			Task<int> task = new Task<int> (() => {
				try {
					using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
						var data = JsonConvert.SerializeObject (obj);
						var s = isoStorage.OpenFile (contentName, FileMode.Create);
						var sw = new StreamWriter (s);
						sw.Write (data);
						sw.Flush ();
						sw.Close ();
					}
					return 1;
				} catch (Exception ex) {
					return 0;
				}

			});
			task.Start ();
			return task;
		}

		public int SaveIsolatedStorage<T> (string contentName, object obj) where T : class
		{

			try {
				using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
					var data = JsonConvert.SerializeObject (obj);
					var s = isoStorage.OpenFile (contentName, FileMode.Create);
					var sw = new StreamWriter (s);
					sw.Write (data);
					sw.Flush ();
					sw.Close ();
				}
				return 1;
			} catch (Exception ex) {
				return 0;
			}
	
		}
	}
}
