using System;
using System.Collections.ObjectModel;

namespace AstroBuilders
{
	public class MenuManager
	{

		public ObservableCollection<MenuGroup> All { get; set; }

		public MenuManager ()
		{
			All = new ObservableCollection<MenuGroup> ();
		}

		private string T(string source) {
			return Translation.GetString (source);
		}
		 
		public void Refresh() {
			All.Clear ();

			MenuGroup mg = new MenuGroup (T("Menu"));
			mg.Add (new Menu (){ Page = MyPage.Home, 		Title = T("MenuNews"), 			Detail = T("MenuNewsDetail"), 			Icon = "news" });
			mg.Add (new Menu (){ Page = MyPage.Builders, 	Title = T("MenuTheBuilders"), 	Detail = T("MenuTheBuildersDetail"), 	Icon = "builders" });
			mg.Add (new Menu (){ Page = MyPage.None, 		Title = T("MenuTheDroids"), 	Detail = T("MenuTheDroidsdetail"), 		Icon = "r2builders" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "AstroDex", 			Detail = "L'anuaire des unités Astro", 		Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, 		Title = T("MenuTheEvents"), 	Detail = T("MenuTheEventsDetail"), 		Icon = "events" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos cartes", 			Detail = "Votre collection de cartes", 		Icon = "IA" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos récompenses", 	Detail = "Vos récompenses obtenues", 		Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.Account, 	Title = T("MenuMyAccount"), 	Detail = T("MenuMyAccountDetail"),		Icon = "builders2" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Aide", 				Detail = "Comment fonctionne l'application", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.About, 		Title = T("MenuAbout"), 		Detail = T("MenuAboutDetail"), 			Icon = "info" });
			All.Add (mg);
			/*
			mg = new MenuGroup ("ASTRODEX");
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Unités R1", Detail = "Astromech de type R1", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Unités R2", Detail = "Astromech de type R2", Icon = "IA" });
			All.Add (mg);
			mg = new MenuGroup ("Récompenses");
			mg.Add (new Menu (){ Page = MyPage.None, Title = "", Detail = "", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "", Detail = "", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "", Detail = "", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Builders Day 06/2015", Detail = "Chez lolo080 - 27 juin", Icon = "lolo080062k15" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "FACTS 2015", Detail = "Gand - 27 & 28 septembre", Icon = "facts2k15" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "PGW 2015", Detail = "Du 28/10 au 1/11", Icon = "pgw2k15" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Comic Con Paris", Detail = "Les 24 & 25 octobre 2015", Icon = "ccp2k15" });
			All.Add (mg);
			*/

			/*
			if (Global.IsConnected && Global.ConnectedUser.IsBuilder) {
				mg = new MenuGroup ("Menu Builders");
				mg.Add (new Menu () { Page = MyPage.None, Title = "Ma fiche", Detail = "Gérer ma fiche de présentation", Icon = "account" });
				mg.Add (new Menu (){ Page = MyPage.None, Title = "Mes robots", 			Detail = "Gérer mes robots", 				Icon = "IA" });
				//mg.Add (new Menu (){ Page = MyPage.None, Title = "", 					Detail = "", 								Icon = "IA" });
				All.Add (mg);
			}
			*/

			if (Global.IsConnected) {
				mg = new MenuGroup (T("MenuMenuBuilders"));
				if (Global.ConnectedUser.IsBuilder) {
					mg.Add (new Menu () { Page = MyPage.MyBuilder, 		Title = T("MenuBuilderPresentation"), 	Detail = T("MenuBuilderPresentationDetail"), 	Icon = "account" });
					mg.Add (new Menu (){ Page = MyPage.None, 			Title = T("MenuBuilderDroids"), 		Detail = T("MenuBuilderDroidsDetail"), 			Icon = "IA" });
					mg.Add (new Menu (){ Page = MyPage.MyExhibitions, 	Title = T("MenuBuilderEvents"), 		Detail = T("MenuBuilderEventsDetail"), 			Icon = "IA" });
				}	
				if (mg.Count > 0)
					All.Add (mg);
				mg = new MenuGroup (T("MenuMenuAdmin"));
				if (Global.ConnectedUser.IsNewser) {
					mg.Add (new Menu () { Page = MyPage.None, 			Title = T("MenuAdminNews"), 	Detail = T("MenuAdminNewsDetail"), 		Icon = "IA" });
				}
				if (Global.ConnectedUser.IsModo) {
					mg.Add (new Menu () { Page = MyPage.None, 			Title = T("MenuAdminAllNews"), 	Detail = T("MenuAdminAllNewsDetail"), 	Icon = "IA" });
				}
				if (Global.ConnectedUser.IsAdmin) {
					mg.Add (new Menu () { Page = MyPage.AdminUsers, 	Title = T("MenuAdminUsers"),	Detail = T("MenuAdminUsersDetail"), 	Icon = "IA" });
				}
				if (Global.ConnectedUser.NickName.Equals("Boris")) {
					mg.Add (new Menu () { Page = MyPage.AdminBuilders, 	Title = "Admin builders", 		Detail = "Gérer qui est builder", 		Icon = "IA" });
				}
				if (mg.Count > 0)
					All.Add (mg);
			}

		}

	}
}