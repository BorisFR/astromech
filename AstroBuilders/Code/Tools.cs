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

		private static HttpClient httpClient;
		private static string res = string.Empty;

		public static string Result { get { return res; } }

		private static bool status = false;

		public static bool StatusDownload { get { return status; } }

		public static void DoInit() {
			if (httpClient != null)
				return;
			httpClient = new HttpClient();
			httpClient.Timeout = new TimeSpan (0, 0, 0, 3, 500);
			httpClient.DefaultRequestHeaders.ExpectContinue = false;
		}

		public static void DoDownload(string fileName) {
			res = string.Empty;
			status = false;
			RealDownload (fileName);
		}

		private static async Task RealDownload(string fileName) {
			try {
				string url = string.Format("{0}Data/{1}", Global.BaseUrl, fileName);
				if(fileName.Equals("users"))
					url = url + "/" + Global.ConnectedUser.Token.ToString();
				System.Diagnostics.Debug.WriteLine("Url: " + url);
				res = await httpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			if (JobDone != null)
				JobDone (status);
			if (DoneBatch != null)
				DoneBatch (status);
		}

		public static void DoCheckUser(User user) {
			res = string.Empty;
			status = false;
			RealDoCheckUser (user);
		}


		private static async Task RealDoCheckUser(User user) {
			try {
				string url = string.Format("{0}Data/CheckUser", Global.BaseUrl);
				//HttpContent content = new StringContent(Helper.Encrypt(JsonConvert.SerializeObject (user)), Encoding.UTF8, "application/json");

				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("login", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent(d);
				//httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				res = await response.Content.ReadAsStringAsync();
				//System.Diagnostics.Debug.WriteLine ("RES: " + res);
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status);
		}
			
		public static void DoCreateUser(User user) {
			res = string.Empty;
			status = false;
			RealDoCreateUser (user);
		}

		private static async Task RealDoCreateUser(User user) {
			try {
				string url = string.Format("{0}Data/CreateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				res = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status);
		}

		public static void DoUpdateUser(User user) {
			res = string.Empty;
			status = false;
			RealDoUpdateUser (user);
		}

		private static async Task RealDoUpdateUser(User user) {
			try {
				string url = string.Format("{0}Data/UpdateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				res = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status);
		}


		public static void DoUpdateUserByAdmin(User user) {
			res = string.Empty;
			status = false;
			RealDoUpdateUserByAdmin (user);
		}

		private static async Task RealDoUpdateUserByAdmin(User user) {
			try {
				string url = string.Format("{0}Data/UpdateBuilderUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (user)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				res = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status);
		}

		public static void DoUpdateBuilder(Builder builder) {
			res = string.Empty;
			status = false;
			RealDoUpdateBuilder (builder);
		}

		private static async Task RealDoUpdateBuilder(Builder builder) {
			try {
				string url = string.Format("{0}Data/UpdateBuilder", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("id", Helper.Encrypt(JsonConvert.SerializeObject (builder)));
				d.Add("token", Global.ConnectedUser.Token.ToString());
				HttpContent content = new FormUrlEncodedContent(d);
				var response = await httpClient.PostAsync(url,content);
				if(response.IsSuccessStatusCode) {
				}
				res = await response.Content.ReadAsStringAsync();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status);
		}

	}
}