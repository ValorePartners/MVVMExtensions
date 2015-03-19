using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.IO;

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

		public Task<bool> SaveIsolatedStorageAsync<T> (string contentName, object obj)
		{
			var task = new Task<bool> (() => {
				try {
					using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication ()) {
						var data = JsonConvert.SerializeObject (obj);
						var s = isoStorage.OpenFile (contentName, FileMode.Create);
						var sw = new StreamWriter (s);
						sw.Write (data);
						sw.Flush ();
						sw.Close ();
					}
					return true;
				} catch (Exception ex) {
					return false;
				}

			});
			task.Start ();
			return task;
		}

		public bool SaveIsolatedStorage<T> (string contentName, object obj) where T : class
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
				return true;
			} catch (Exception ex) {
				return false;
			}

		}
	}

}
