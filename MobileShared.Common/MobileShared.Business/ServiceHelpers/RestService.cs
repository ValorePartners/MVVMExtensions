using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace  MobileShared.Business
{

	public class RestService
	{
		public TimeSpan Timeout{ get; set; }

		public RestService () : this (60)
		{
			
		}

		public RestService (int seconds)
		{
			Timeout = new TimeSpan (0, 0, seconds);
		}

		public async Task<T> GetAsync<T> (string url) where T : new()
		{
			return await Task.Factory.StartNew (async () => {
			
				var client = new HttpClient ();
				var response = client.GetAsync (url).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
	
			}).Unwrap ();
		}

		public async Task<T> GetAsync<T> (string url, NetworkCredential credentials) where T : new()
		{
			return await Task.Factory.StartNew (async () => {
			
				var handler = new HttpClientHandler ();
				handler.Credentials = credentials;
				HttpClient client = new HttpClient (handler);
				var response = client.GetAsync (url).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
		
			}).Unwrap ();
		}

		public async Task<T> PostAsync<T> (string url, object obj) where T : new()
		{
			return await Task.Factory.StartNew (async () => {
			
				var client = new HttpClient (new HttpClientHandler ());
				var data = JsonConvert.SerializeObject (obj);
				var response = client.PostAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
		
			}).Unwrap ();

		}

		public async Task<T> PostAsync<T> (string url, object obj, NetworkCredential credentials) where T : new()
		{
			return await Task.Factory.StartNew (async () => {

				var handler = new HttpClientHandler ();
				handler.Credentials = credentials;
				var client = new HttpClient (handler);
				var data = JsonConvert.SerializeObject (obj);
				var response = client.PostAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
		
			}).Unwrap ();
		}

		public async Task<T> PutAsync<T> (string url, object obj, NetworkCredential credentials) where T : new()
		{
			return await Task.Factory.StartNew (async () => {
			
				var handler = new HttpClientHandler ();
				handler.Credentials = credentials;
				var client = new HttpClient (handler);
				var data = JsonConvert.SerializeObject (obj);
				var response = client.PutAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
		
			}).Unwrap ();
		}

		public async Task<T> PutAsync<T> (string url, object obj) where T : new()
		{
			return await Task.Factory.StartNew (async () => {
		
				var client = new HttpClient (new HttpClientHandler ());
				var data = JsonConvert.SerializeObject (obj);
				var response = client.PutAsync (url, new StringContent (data, Encoding.UTF8, "application/json")).Result;
				var jsonResult = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<T> (jsonResult);
		
			}).Unwrap ();
		}

		public async Task<T> GetAsync<T> (string url, string userName, string password, string domain) where T : new()
		{
           
			return await GetAsync<T> (url, new NetworkCredential (userName, password, domain));
		}

		public async Task<T> PostAsync<T> (string url, object obj, string userName, string password, string domain) where T : new()
		{
           
			return await PostAsync<T> (url, obj, new NetworkCredential (userName, password, domain));
		}

		public async Task<T> PutAsync<T> (string url, object obj, string userName, string password, string domain) where T : new()
		{

			return await PutAsync<T> (url, obj, new NetworkCredential (userName, password, domain));
		}

	}
}
