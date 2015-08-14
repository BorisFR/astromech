using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using AstroBuildersModel;
using System.Collections.ObjectModel;

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

		public JsonResult Exhibitions() {
			return new JsonNetResult (Helper.AllExhibitions.All);
		}

		public JsonResult Users(string id) {
			try {
				Guid toTest = new Guid (id);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (u.IsAdmin) {
							List<User> list = new List<User>();
							foreach(User user in Helper.AllUsers.All) {
								if(user.IdClub.Equals(Guid.Empty) || user.IdClub.Equals(u.IdClub))
									list.Add(user);
							}
							return new JsonNetResult (list);
						} else
							return new JsonNetResult (null);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (null);
		}

		public JsonResult ReloadData() {
			Helper.ReloadData ();
			return new JsonNetResult (null);
		}

		public JsonResult UpdateBuilderUser(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				string data = Helper.Decrypt (id);
				User user = JsonConvert.DeserializeObject<User> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (u.IsAdmin) {
							foreach (User old in Helper.AllUsers.Collection) {
								if (old.Id == user.Id) {
									// petite vérif complémentaire
									if (old.NickName.Equals (user.NickName) && old.Login.Equals (user.Login)) {
										if((old.IsBuilder != user.IsBuilder) && user.IsBuilder) {
											// we need to create a new builder

											Builder b = new Builder ();
											b.Id = user.IdBuilder;
											b.IdClub = u.IdClub;
											b.Title = old.Title;
											b.NickName = old.NickName;
											b.Email = old.Email;
											Helper.AllBuilders.Add (b);

											//Tools.SaveTextFile (Helper.AllBuilders.CollectionName, Helper.AllBuilders.Save ());

										}
										old.IdClub = u.IdClub;
										old.IdBuilder = user.IdBuilder;
										old.IsAdmin = user.IsAdmin;
										old.IsModo = user.IsModo;
										old.IsNewser = user.IsNewser;
										Helper.AllUsers.Update(old);
										//Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
										return new JsonNetResult (Helper.AllUsers.All);
									} else {
										return new JsonNetResult (null);
									}
								}
							}
							return new JsonNetResult (null);
						} else
							return new JsonNetResult (null);
					}
				}
			} catch (Exception) {
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
				//Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
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
							Helper.AllUsers.Update(u);
							//Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
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
				string data = Helper.Decrypt (id);
				User user = JsonConvert.DeserializeObject<User> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (u.Id.Equals (user.Id)) {
							foreach (User old in Helper.AllUsers.Collection) {
								if (old.Id.Equals (user.Id) && old.Password.Equals(user.Password) && old.Login.Equals(user.Login)) {
									//old.NickName = user.NickName;
									old.Email = user.Email;
									old.IdCountry = user.IdCountry;
									Helper.AllUsers.Update(old);
									//Tools.SaveTextFile (Helper.AllUsers.CollectionName, Helper.AllUsers.Save ());
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


		public JsonResult UpdateBuilder(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				string data = Helper.Decrypt (id);
				Builder builder = JsonConvert.DeserializeObject<Builder> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (u.IdBuilder.Equals (builder.Id)) {
							Builder old = (Builder)Helper.AllBuilders.GetByGuid<Builder>(builder.Id);
							old.Blog = builder.Blog;
							old.Detail = builder.Detail;
							old.Droids = builder.Droids;
							old.Email = builder.Email;
							old.Facebook = builder.Facebook;
							old.Location = builder.Location;
							old.Logo = builder.Logo;
							old.NickName = builder.NickName;
							old.Title = builder.Title;
							Helper.AllBuilders.Update(old);
							//Tools.SaveTextFile (Helper.AllBuilders.CollectionName, Helper.AllBuilders.Save ());
							return new JsonNetResult (Helper.AllBuilders.All);
						}
						return new JsonNetResult (false);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (false);
		}


		public JsonResult CreateExhibition(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				string data = Helper.Decrypt (id);
				Exhibition exhibition = JsonConvert.DeserializeObject<Exhibition> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if(!u.IsBuilder)
							return new JsonNetResult (false);
						Helper.AllExhibitions.Add(exhibition);
						return new JsonNetResult (Helper.AllExhibitions.All);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (false);
		}


		public JsonResult DeleteExhibition(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				string data = Helper.Decrypt (id);
				Exhibition exhibition = JsonConvert.DeserializeObject<Exhibition> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (!u.IsBuilder)
							return new JsonNetResult (false);
						bool found = false;
						foreach (Exhibition e in Helper.AllExhibitions.All) {
							if (e.Id.Equals (exhibition.Id)) {
								found = true;
								Tools.DeleteTextFile (Helper.AllExhibitions.FolderName, e.Id.ToString ());
								break;
							}
						}
						if (found) {
							Helper.AllExhibitions.Collection.Clear ();
							return new JsonNetResult (Helper.AllExhibitions.All);
						}
						return new JsonNetResult (false);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (false);
		}

		public JsonResult UpdateExhibition(string id, string token) {
			try {
				Guid toTest = new Guid (token);
				string data = Helper.Decrypt (id);
				Exhibition exhibition = JsonConvert.DeserializeObject<Exhibition> (data);
				foreach (User u in Helper.AllUsers.Collection) {
					if (u.Token == toTest) {
						if (!u.IsBuilder)
							return new JsonNetResult (false);
						foreach (Exhibition e in Helper.AllExhibitions.All) {
							if (e.Id.Equals (exhibition.Id)) {
								e.Title = exhibition.Title;
								e.Builders = exhibition.Builders;
								e.Description = exhibition.Description;
								e.EndDate = exhibition.EndDate;
								e.Flyer = exhibition.Flyer;
								e.IdBuilder = exhibition.IdBuilder;
								e.IdCountry = exhibition.IdCountry;
								e.Logo = exhibition.Logo;
								e.StartDate = exhibition.StartDate;
								Helper.AllExhibitions.Update (e);
								return new JsonNetResult (Helper.AllExhibitions.All);
							}
						}
						return new JsonNetResult (false);
					}
				}
			} catch (Exception) {
			}
			return new JsonNetResult (false);
		}

	}
}