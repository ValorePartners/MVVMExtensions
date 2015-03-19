using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace  MobileRef.MVVM.Shared
{
	public delegate void RestServiceException (Exception ex);
	public class RestService
	{
		public TimeSpan Timeout{ get; set; }

		public event RestServiceException ExceptionOccured;

		public RestService () : this (60)
		{
			
		}

		public RestService (int seconds)
		{
			Timeout = new TimeSpan (0, 0, seconds);
		}

		private HttpClient GetClient (NetworkCredential credentials)
		{
			#if __ANDROID__
			var handler = new ModernHttpClient.NativeMessageHandler ();
			if (credentials != null) {
				handler.Credentials = credentials;
			}
			return new HttpClient (handler){ Timeout = this.Timeout };
			#elif __IOS__
			var handler = new ModernHttpClient.NativeMessageHandler ();
			if (credentials != null) {
				handler.Credentials = credentials;
			}
			return new HttpClient (handler){Timeout = this.Timeout};
			#else
			var handler = new HttpClientHandler ();
			if(credentials!=null)
				handler.Credentials = credentials;
			return new HttpClient(handler){Timeout = this.Timeout};
			#endif

		}

		public async Task<T> GetAsync<T> (string url) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (null)) {
						var response = client.GetAsync (url).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
			});
		}

		public async Task<T> GetAsync<T> (string url, NetworkCredential credentials) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (credentials)) {
						var response = client.GetAsync (url).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
			});
		}

		public async Task<T> PostAsync<T> (string url, object obj) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (null)) {
						var data = JsonConvert.SerializeObject (obj);
						var response = client.PostAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
			});

		}

		public async Task<T> PostAsync<T> (string url, object obj, NetworkCredential credentials) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (credentials)) {
						var data = JsonConvert.SerializeObject (obj);
						var response = client.PostAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
	
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
		
			});
		}

		public async Task<T> PutAsync<T> (string url, object obj, NetworkCredential credentials) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (credentials)) {
						var data = JsonConvert.SerializeObject (obj);
						var response = client.PutAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
			});
		}

		public async Task<T> PutAsync<T> (string url, object obj) where T : class, new()
		{
			return await Task.Run (async () => {
				try {
					using (var client = GetClient (null)) {
						var data = JsonConvert.SerializeObject (obj);
						var response = client.PutAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
						var jsonResult = await response.Content.ReadAsStringAsync ();
						return JsonConvert.DeserializeObject<T> (jsonResult);
					}
				} catch (Exception ex) {
					if (ExceptionOccured != null)
						ExceptionOccured (ex);
					return null;
				}
			});
		}

		public async Task<T> GetAsync<T> (string url, string userName, string password, string domain) where T : class, new()
		{  
			return await GetAsync<T> (url, new NetworkCredential (userName, password, domain));
		}

		public async Task<T> PostAsync<T> (string url, object obj, string userName, string password, string domain) where T : class, new()
		{
			return await PostAsync<T> (url, obj, new NetworkCredential (userName, password, domain));
		}

		public async Task<T> PutAsync<T> (string url, object obj, string userName, string password, string domain) where T : class, new()
		{
			return await PutAsync<T> (url, obj, new NetworkCredential (userName, password, domain));
		}

	}

}
