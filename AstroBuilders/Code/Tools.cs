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

		public static void Trace (string text)
		{
			System.Diagnostics.Debug.WriteLine (text);
		}

		public static void DoInit ()
		{
		}

		private static HttpClient httpClient = null;
		//new HttpClient ();

		private static HttpClient theHttpClient {
			get {
				if (httpClient != null)
					return httpClient;
				httpClient = new HttpClient ();
				Trace ("----------Max response size: " + httpClient.MaxResponseContentBufferSize.ToString ());
				httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				return httpClient;
			}
		}


		public static async Task DoDownload (string fileName)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/{1}", Global.BaseUrl, fileName);
				if (fileName.Equals ("users"))
					url = url + "/" + Global.ConnectedUser.Token.ToString ();
				System.Diagnostics.Debug.WriteLine ("Url: " + url);
				result = await theHttpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			if (JobDone != null)
				JobDone (status, result);
			if (DoneBatch != null)
				DoneBatch (status, result);
		}

		public static async Task DoCheckUser (User user)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/CheckUser", Global.BaseUrl);

				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("login", Helper.Encrypt (JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent (d);

				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static async Task DoCreateUser (User user)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/CreateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("id", Helper.Encrypt (JsonConvert.SerializeObject (user)));
				HttpContent content = new FormUrlEncodedContent (d);
				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static async Task DoUpdateUser (User user)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/UpdateUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("id", Helper.Encrypt (JsonConvert.SerializeObject (user)));
				d.Add ("token", Global.ConnectedUser.Token.ToString ());
				HttpContent content = new FormUrlEncodedContent (d);
				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}


		public static void DoUpdateUserByAdmin (User user)
		{
			RealDoUpdateUserByAdmin (user);
		}

		private static async Task RealDoUpdateUserByAdmin (User user)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/UpdateBuilderUser", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("id", Helper.Encrypt (JsonConvert.SerializeObject (user)));
				d.Add ("token", Global.ConnectedUser.Token.ToString ());
				HttpContent content = new FormUrlEncodedContent (d);
				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static void DoUpdateBuilder (Builder builder)
		{
			RealDoUpdateBuilder (builder);
		}

		private static async Task RealDoUpdateBuilder (Builder builder)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/UpdateBuilder", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("id", Helper.Encrypt (JsonConvert.SerializeObject (builder)));
				d.Add ("token", Global.ConnectedUser.Token.ToString ());
				HttpContent content = new FormUrlEncodedContent (d);
				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}

		public static void DoCreateExhibition (Exhibition exhibition)
		{
			RealDoCreateExhibition (exhibition);
		}

		private static async Task RealDoCreateExhibition (Exhibition exhibition)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ("{0}Data/CreateExhibition", Global.BaseUrl);
				Dictionary<string, string> d = new Dictionary<string, string> ();
				d.Add ("id", Helper.Encrypt (JsonConvert.SerializeObject (exhibition)));
				d.Add ("token", Global.ConnectedUser.Token.ToString ());
				HttpContent content = new FormUrlEncodedContent (d);
				var response = await theHttpClient.PostAsync (url, content);
				if (response.IsSuccessStatusCode) {
				}
				result = await response.Content.ReadAsStringAsync ();
				status = true;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			JobDone (status, result);
		}


		public static async Task<string> PostMultiPartForm (string url, byte[] file, string paramName, string contentType, Dictionary<String, string> nvc,
		                                                    string cookie)
		{
			try {
				string responseString = string.Empty;
				HttpContent bytesContent = new ByteArrayContent (file);
				bytesContent.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
					FileName = '"' + contentType + '"',
					Name = '"' + paramName + '"' 
				};
				bytesContent.Headers.ContentType = new MediaTypeHeaderValue ("image/jpeg");
				//bytesContent.Headers.ContentType = new MediaTypeHeaderValue ('"' + "multipart/form-data" + '"');

				//using (var client = theHttpClient)
				//using (var formData = new MultipartFormDataContent ()) {
				var formData = new MultipartFormDataContent ();
				formData.Add (bytesContent, '"' + paramName + '"', '"' + contentType + '"');
				HttpResponseMessage response = null;
				try {
					Trace ("Post");
					response = theHttpClient.PostAsync (url, formData).Result;
					//response = await theHttpClient.PostAsync (url, formData);
					Trace ("input");
					responseString = await response.Content.ReadAsStringAsync ();
					Trace (responseString);
					try {
						string json = Helper.Decrypt (responseString);
						Trace ("JSON: " + json);
					} catch (Exception) {
					}
					if (!response.IsSuccessStatusCode) {
						Trace (response.ToString ());
						if (JobDone != null)
							JobDone (false, string.Empty);
						return string.Empty;
					}
					//return response.Content.ReadAsStreamAsync ().Result;
				} catch (Exception err) {
					Trace (err.Message);
					if (JobDone != null)
						JobDone (false, string.Empty);
					return string.Empty;
				}
				/*
				try {
					Stream respStream = response.Content.ReadAsStreamAsync ().Result;
					StreamReader respReader = new StreamReader (respStream);
					responseString = respReader.ReadToEnd ();
					//log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
					Trace ("Response: " + responseString);
				} catch (Exception ex) {
					//log.Error("Error uploading file", ex);
					Trace ("Error uploading file: " + ex.Message);
				}
				*/
				formData = null;
				if (JobDone != null)
					JobDone (true, responseString);
				return responseString;
			} catch (Exception error) {
				Trace (error.Message);
			}
			if (JobDone != null)
				JobDone (false, string.Empty);
			return string.Empty;
			/*
			// log.Debug(string.Format("Uploading {0} to {1}", file, url));
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString ("x");
			byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes ("\r\n--" + boundary + "\r\n");

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create (url);
			wr.ContentType = "multipart/form-data; boundary=" + boundary;
			wr.Method = "POST";
			if (cookie.Length > 0)
				wr.Headers ["Cookie"] = cookie;
			//wr.KeepAlive = true;
			//wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

			Stream rs = await wr.GetRequestStreamAsync ();

			string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			foreach (string key in nvc.Keys) {
				rs.Write (boundarybytes, 0, boundarybytes.Length);
				string formitem = string.Format (formdataTemplate, key, nvc [key]);
				byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes (formitem);
				rs.Write (formitembytes, 0, formitembytes.Length);
			}
			rs.Write (boundarybytes, 0, boundarybytes.Length);

			string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			string header = string.Format (headerTemplate, paramName, file, contentType);
			byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes (header);
			rs.Write (headerbytes, 0, headerbytes.Length);

			rs.Write (file, 0, file.Length);


			byte[] trailer = System.Text.Encoding.UTF8.GetBytes ("\r\n--" + boundary + "--\r\n");
			rs.Write (trailer, 0, trailer.Length);
			//rs.Close();
			string responseString = String.Empty;
			WebResponse wresp = null;
			try {
				wresp = await wr.GetResponseAsync ();
				Stream respStream = wresp.GetResponseStream ();
				StreamReader respReader = new StreamReader (respStream);
				responseString = respReader.ReadToEnd ();
				//log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
				System.Diagnostics.Debug.WriteLine ("Response: " + responseString);
			} catch (Exception ex) {
				//log.Error("Error uploading file", ex);
				System.Diagnostics.Debug.WriteLine ("Error uploading file: " + ex.Message);
				if (wresp != null) {
					//wresp.Close();
					wresp = null;
				}
			} finally {
				wr = null;
			}
			return responseString;
			*/
		}

	}

	public static class WebRequestExtensions
	{
		public static Task<WebResponse> GetResponseAsync (this WebRequest request)
		{
			return Task.Factory.StartNew<WebResponse> (() => {
				var t = Task.Factory.FromAsync<WebResponse> (
					        request.BeginGetResponse,
					        request.EndGetResponse,
					        null);

				t.Wait ();

				return t.Result;
			});
		}

		public static Task<Stream> GetRequestStreamAsync (this WebRequest request)
		{
			return Task.Factory.StartNew<Stream> (() => {
				var t = Task.Factory.FromAsync<Stream> (
					        request.BeginGetRequestStream,
					        request.EndGetRequestStream,
					        null);

				t.Wait ();

				return t.Result;
			});
		}

	}
}