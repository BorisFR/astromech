using System;
using System.Collections.ObjectModel;

namespace AstroBuildersModel
{

	public class NewsManager : IManager<News> { 

		public ObservableCollection<NewsGroup> AllNewsGroup { get; set; }
		public void Refresh() {
			//AllNewsGroup.Clear ();
			//All.Sort ();
			AllNewsGroup.Clear ();
			int year = 0;
			NewsGroup ng = null;
			foreach (News news in All) {
				// do not show future news :)
				if (news.Date > DateTime.Now)
					continue;
				if (news.Year != year) {
					if (ng != null)
						AllNewsGroup.Add (ng);
					ng = new NewsGroup (news.Year.ToString ());
					year = news.Year;
				}
				ng.Add (news);
			}
			if (ng != null)
				AllNewsGroup.Add (ng);
		}
		public NewsManager() : base("news.json") {AllNewsGroup = new ObservableCollection<NewsGroup> ();} 
	}

	public class CountryManager : IManager<Country> { public CountryManager() : base("country.json") {}}
	public class BuildersManager : IManager<Builder> { public BuildersManager() : base("builders.json") {}}
	public class ClubsManager : IManager<Club> { public ClubsManager() : base("clubs.json") {}}
	public class UsersManager : IManager<User> { public UsersManager() : base("users.json") {}}
	public class ExhibitionsManager : IManager<Exhibition> { public ExhibitionsManager() : base("exhibitions.json") {}}

}