using System;
using System.Net.Http;
using System.Threading.Tasks;
using AstroBuildersModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace AstroBuilders
{
	
	public static class Tools
	{
		public static event JobDone JobDone;
		public static event JobDone DoneBatch;

		public static void DoInit() {
		}

		private static HttpClient httpClient {
			get{
				HttpClient httpClient = new HttpClient ();
				httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				return httpClient;
			}
		}


		public static void DoDownload(string fileName) {
			RealDownload (fileName);
		}

		private static async Task RealDownload(string fileName) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/{1}", Global.BaseUrl, fileName);
				if(fileName.Equals("users"))
					url = url + "/" + Global.ConnectedUser.Token.ToString();
				System.Diagnostics.Debug.WriteLine("Url: " + url);
				result = await httpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			if (JobDone != null)
				JobDone (status, result);
			if (DoneBatch != null)
				DoneBatch (status, result);
		}

		public static void DoCheckUser(User user) {
			RealDoCheckUser (user);
		}


		private static async Task RealDoCheckUser(User user) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/CheckUser", Global.BaseUrl);

				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("login", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent(d);

				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}
			
		public static void DoCreateUser(User user) {
			RealDoCreateUser (user);
		}

		private static async Task RealDoCreateUser(User user) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/CreateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static void DoUpdateUser(User user) {
			RealDoUpdateUser (user);
		}

		private static async Task RealDoUpdateUser(User user) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/UpdateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}


		public static void DoUpdateUserByAdmin(User user) {
			RealDoUpdateUserByAdmin (user);
		}

		private static async Task RealDoUpdateUserByAdmin(User user) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/UpdateBuilderUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static void DoUpdateBuilder(Builder builder) {
			RealDoUpdateBuilder (builder);
		}

		private static async Task RealDoUpdateBuilder(Builder builder) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/UpdateBuilder", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (builder)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static void DoCreateExhibition(Exhibition exhibition) {
			RealDoCreateExhibition (exhibition);
		}

		private static async Task RealDoCreateExhibition(Exhibition exhibition) {
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format("{0}Data/CreateExhibition", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (exhibition)));
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

	}
}