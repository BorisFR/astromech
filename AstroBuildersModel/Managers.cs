using System;
using System.Collections.ObjectModel;

namespace AstroBuildersModel
{

	public class NewsManager : IManager<News>
	{

		public ObservableCollection<NewsGroup> AllNewsGroup { get; set; }

		public void Refresh ()
		{
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

		public NewsManager () : base ("news.json")
		{
			AllNewsGroup = new ObservableCollection<NewsGroup> ();
		}
	}

	public class ExhibitionsManager : IManager<Exhibition>
	{

		public ObservableCollection<ExhibitionsGroup> AllExhibitionsGroup { get; set; }

		public void Refresh ()
		{
			AllExhibitionsGroup.Clear ();
			int year = 0;
			ExhibitionsGroup ng = null;
			foreach (Exhibition o in All) {
				if (o.Year != year) {
					if (ng != null)
						AllExhibitionsGroup.Add (ng);
					ng = new ExhibitionsGroup (o.Year.ToString ());
					year = o.Year;
				}
				ng.Add (o);
			}
			if (ng != null)
				AllExhibitionsGroup.Add (ng);
		}

		private ObservableCollection<Exhibition> allFromBuilder = null;
		private Guid currentBuilderId = Guid.Empty;

		public ObservableCollection<Exhibition> AllFromBuilder (Guid id)
		{
			if (currentBuilderId != Guid.Empty && currentBuilderId.Equals (id) && allFromBuilder != null)
				return allFromBuilder;
			currentBuilderId = id;
			allFromBuilder = new ObservableCollection<Exhibition> ();
			foreach (Exhibition o in All) {
				if (o.IdBuilder.Equals (id))
					allFromBuilder.Add (o);
			}
			return allFromBuilder;
		}

		public ExhibitionsManager () : base ("exhibitions.json")
		{
			AllExhibitionsGroup = new ObservableCollection<ExhibitionsGroup> ();
		}
	}


	public class CountryManager : IManager<Country>
	{
		public CountryManager () : base ("country.json")
		{
		}

	}

	public class BuildersManager : IManager<Builder>
	{
		public BuildersManager () : base ("builders.json")
		{
		}

	}

	public class ClubsManager : IManager<Club>
	{
		public ClubsManager () : base ("clubs.json")
		{
		}

	}

	public class UsersManager : IManager<User>
	{
		public UsersManager () : base ("users.json")
		{
		}

	}

	public class CardsManager : IManager<Card>
	{
		public CardsManager () : base ("cards.json")
		{
		}

	}

}