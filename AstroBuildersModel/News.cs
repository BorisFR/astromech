using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace AstroBuildersModel
{

	public class NewsGroup : ObservableCollection<News>
	{

		public NewsGroup(string title) {
			Title = title;
		}

		public string Title { get; private set; }

	}

	public class News : IModel, IComparable<News>
	{
		private Guid idClub = Guid.Empty;
		public Guid IdClub {
			get { return idClub; }
			set {
				if (value.Equals (idClub))
					return;
				idClub = value;
				OnPropertyChanged ("IdClub");
			}
		}

		private Guid idBuilder = Guid.Empty;
		public Guid IdBuilder {
			get { return idBuilder; }
			set {
				if (value.Equals (idBuilder))
					return;
				idBuilder = value;
				OnPropertyChanged ("IdBuilder");
			}
		}

		private DateTime date = new DateTime (2000, 1, 1);
		public DateTime Date {
			get { return date; }
			set {
				if (value.Equals (date))
					return;
				date = value;
				OnPropertyChanged ("Date");
			}
		}


		private string detail = string.Empty;
		public string Detail {
			get { return detail; }
			set {
				if (value.Equals (detail))
					return;
				detail = value;
				OnPropertyChanged ("Detail");
			}
		}

		[IgnoreDataMember]
		public int Year {
			get { return date.Year; }
		}

		private string builderNickName = string.Empty;
		[IgnoreDataMember]
		public string BuilderNickname {
			get { return builderNickName; }
			set{ builderNickName = value; }
		}

		private string clubName = string.Empty;
		[IgnoreDataMember]
		public string ClubName {
			get { return clubName; }
			set{ clubName = value; }
		}
			
		// newest date first
		public int CompareTo (News other) {
			return other.Date.CompareTo (this.Date);
		}
	}
}