using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using AstroBuildersModel;

namespace AstroWeb.Controllers
{
    public class DataController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

		public JsonResult Country() {
			return new JsonNetResult (Helper.AllCountry.All);
		}

		public JsonResult News() {
			return new JsonNetResult (Helper.AllNews.All);
		}

		public JsonResult Builders() {
			return new JsonNetResult (Helper.AllBuilders.All);
		}
	
		public JsonResult Clubs() {
			return new JsonNetResult (Helper.AllClubs.All);
		}

		public JsonResult Users(string id) {
			try{
			Guid toTest = new Guid (id);
			foreach (User u in Helper.AllUsers.Collection) {
				if (u.Token == toTest) {
					if (u.IsAdmin)
						return new JsonNetResult (Helper.AllUsers.All);
					else
						return new JsonNetResult (null);
				}
			}
			} catch(Exception){
			}
			return new JsonNetResult (null);
		}

		public JsonResult ReloadData() {
			Helper.ReloadData ();
			return new JsonNetResult (null);
		}

		public JsonResult UpdateBuilderUser(string id, string token) {
			Guid toTest = new Guid (token);
			foreach (User u in Helper.AllUsers.Collection) {
				if (u.Token == toTest) {
					if (u.IsAdmin) {
						string data = Helper.Decrypt (id);
						User user = JsonConvert.DeserializeObject<User> (data);
						foreach (User old in Helper.AllUsers.Collection) {
							if (old.Id == user.Id) {
								// petite vérif complémentaire
								if (old.NickName.Equals (user.NickName) && old.Login.Equals (user.Login)) {
									old.IdBuilder = user.IdBuilder;
									Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
									return new JsonNetResult (Helper.AllUsers.All);
								} else {
									return new JsonNetResult (null);
								}
							}
						}
						return new JsonNetResult (null);
					}
					else
						return new JsonNetResult (null);
				}
			}
			return new JsonNetResult (null);
		}

		public JsonResult CreateUser(string id) {
			try {
				string data = Helper.Decrypt (id);
				User user = JsonConvert.DeserializeObject<User> (data);
				if(string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NickName)) {
					user.Title = "§0 missing info";
					return new JsonNetResult (user);
				}
				foreach (User u in Helper.AllUsers.Collection) {
					if (user.Login.Equals (u.Login)) {
						user.Title = "§1 login already exist";
						return new JsonNetResult (user);
					}
					if (user.NickName.Equals (u.NickName)) {
						user.Title = "§2 nickName already exist";
						return new JsonNetResult (user);
					}
				}
				user.CountLogin = 1;
				user.LastConnected = DateTime.UtcNow;
				Helper.AllUsers.Add(user);
				Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
				return new JsonNetResult (user);
			} catch (Exception) {
			}
			return new JsonNetResult (null);
		}

		[HttpPost]
		public JsonResult CheckUser(string login) {
			try {
				string data = Helper.Decrypt (login);
				User user = JsonConvert.DeserializeObject<User> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (user.Login.Equals (u.Login)) {
						if (user.Password.Equals (u.Password)) {
							u.CountLogin++;
							u.LastConnected = DateTime.UtcNow;
							u.Token = Guid.NewGuid();
							Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
							return new JsonNetResult (u);
						}
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (null);
		}


		public JsonResult UpdateUser(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						string data = Helper.Decrypt (id);
						User user = JsonConvert.DeserializeObject<User> (data);
						if (u.Id.Equals (user.Id)) {
							foreach (User old in Helper.AllUsers.Collection) {
								if (old.Id.Equals (user.Id)) {
									//old.NickName = user.NickName;
									old.Email = user.Email;
									old.IdCountry = user.IdCountry;
									Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
									return new JsonNetResult (old);
								}
							}
							return new JsonNetResult (false);
						} else
							return new JsonNetResult (false);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (false);
		}

	}
}