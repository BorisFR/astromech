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
		 
		public void Refresh() {
			All.Clear ();

			MenuGroup mg = new MenuGroup ("");
			mg.Add (new Menu (){ Page = MyPage.Home, Title = "Actualités", 			Detail = "Les actus de l'association", 		Icon = "news" });
			mg.Add (new Menu (){ Page = MyPage.Builders, Title = "Les Constructeurs", Detail = "Les membres de l'association", 	Icon = "builders" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Les robots", 			Detail = "Les robots des R2 Builders", 		Icon = "r2builders" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "AstroDex", 			Detail = "L'anuaire des unités Astro", 		Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.None, Title = "Les événements", 		Detail = "Les R2 Builders en balade", 		Icon = "events" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos cartes", 			Detail = "Votre collection de cartes", 		Icon = "IA" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos récompenses", 	Detail = "Vos récompenses obtenues", 		Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.Account, Title = "Mon compte", 		Detail = "Gérer mes informations",			Icon = "builders2" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Aide", 				Detail = "Comment fonctionne l'application", Icon = "IA" });
			mg.Add (new Menu (){ Page = MyPage.About, Title = "A propos de", 		Detail = "Informations", 					Icon = "info" });
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
				mg = new MenuGroup ("Menu Builder");
				if (Global.ConnectedUser.IsBuilder) {
					mg.Add (new Menu () { Page = MyPage.MyBuilder, Title = "Ma fiche", 			Detail = "Gérer ma fiche de présentation", 	Icon = "account" });
					mg.Add (new Menu (){ Page = MyPage.None, Title = "Mes robots", 			Detail = "Gérer mes robots", 				Icon = "IA" });
				}	
				if (mg.Count > 0)
					All.Add (mg);
				mg = new MenuGroup ("Administration");
				if (Global.ConnectedUser.IsNewser) {
					mg.Add (new Menu () { Page = MyPage.None, Title = "Mes actualités", 	Detail = "Gérer mes actualités", 			Icon = "IA" });
				}
				if (Global.ConnectedUser.IsModo) {
					mg.Add (new Menu () { Page = MyPage.None, Title = "Toutes les actualités", Detail = "Gérer toutes les actualités", 	Icon = "IA" });
				}
				if (Global.ConnectedUser.IsAdmin) {
					mg.Add (new Menu () { Page = MyPage.AdminUsers, Title = "Admin Users", 	Detail = "Gérer les utilisateurs", 			Icon = "IA" });
				}
				if (Global.ConnectedUser.NickName.Equals("Boris")) {
					mg.Add (new Menu () { Page = MyPage.AdminBuilders, Title = "Admin builders", Detail = "Gérer qui est builder", 		Icon = "IA" });
				}
				if (mg.Count > 0)
					All.Add (mg);
			}

		}

	}
}