using System;
using System.Collections.Generic;

namespace AstroBuildersModel
{
	public class Builder : IModel, IComparable<Builder>
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

		private string email = string.Empty;
		public string Email {
			get { return email; }
			set {
				if (value.Equals (email))
					return;
				email = value;
				OnPropertyChanged ("Email");
			}
		}

		private string facebook = string.Empty;
		public string Facebook {
			get { return facebook; }
			set {
				if (value.Equals (facebook))
					return;
				facebook = value;
				OnPropertyChanged ("Facebook");
			}
		}

		private string blog = string.Empty;
		public string Blog {
			get { return blog; }
			set {
				if (value.Equals (blog))
					return;
				blog = value;
				OnPropertyChanged ("Blog");
			}
		}

		private string location = string.Empty;
		public string Location {
			get { return location; }
			set {
				if (value.Equals (location))
					return;
				location = value;
				OnPropertyChanged ("Location");
			}
		}

		private string droids = string.Empty;
		public string Droids {
			get { return droids; }
			set {
				if (value.Equals (droids))
					return;
				droids = value;
				OnPropertyChanged ("Droids");
			}
		}

		private string idForum = string.Empty;
		public string IdForum {
			get { return idForum; }
			set {
				if (value.Equals (idForum))
					return;
				idForum = value;
				OnPropertyChanged ("IdForum");
			}
		}

		private string nickName = string.Empty;
		public string NickName {
			get { return nickName; }
			set {
				if (value.Equals (nickName))
					return;
				nickName = value;
				OnPropertyChanged ("NickName");
			}
		}

		private string logo = string.Empty;
		public string Logo {
			get { return logo; }
			set {
				if (value.Equals (logo))
					return;
				logo = value;
				OnPropertyChanged ("Logo");
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

		private List<string> otherInfo = new List<string>();
		public List<string> OtherInfo {
			get { return otherInfo; }
			set {
				if (value.Equals (otherInfo))
					return;
				otherInfo = value;
				OnPropertyChanged ("OtherInfo");
			}
		}


		public Builder DeepCopy()
		{
			Builder othercopy = (Builder)this.MemberwiseClone();
			return othercopy;
		}

		public int CompareTo (Builder other) {
			return -other.NickName.CompareTo (this.NickName);
		}

	}
}