using System;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AstroWeb
{
	public class JsonNetResult : JsonResult
	{
		public JsonNetResult ()
		{
			this.ContentType = "application/json";
		}

		public JsonNetResult(object data)
		{
			this.ContentEncoding = Encoding.UTF8;
			this.ContentType = "application/json";
			this.Data = data;
			this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
		}

		public JsonNetResult(object data, JsonRequestBehavior behavior)
		{
			this.ContentEncoding = Encoding.UTF8;
			this.ContentType = "application/json";
			this.Data = data;
			this.JsonRequestBehavior = behavior;
		}

		public JsonNetResult(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior jsonRequestBehavior)
		{
			this.ContentEncoding = contentEncoding;
			this.ContentType = !string.IsNullOrWhiteSpace(contentType) ? contentType : "application/json";
			this.Data = data;
			this.JsonRequestBehavior = jsonRequestBehavior;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var response = context.HttpContext.Response;

			response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null)
				return;

			// If you need special handling, you can call another form of SerializeObject below
			JsonSerializerSettings jsonSettings = new JsonSerializerSettings ();
			jsonSettings.NullValueHandling = NullValueHandling.Ignore;
			jsonSettings.Converters.Add (new StringEnumConverter ());
			var serializedObject = JsonConvert.SerializeObject(Data, Formatting.None, jsonSettings);
			//response.Write(serializedObject);
			response.Write(Helper.Encrypt(serializedObject));
		}

	}
}