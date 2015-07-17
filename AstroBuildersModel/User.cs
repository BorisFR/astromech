using System;
using System.Runtime.Serialization;

namespace AstroBuildersModel
{
	public class User : IModel, IComparable<User>
	{

		private Guid token = Guid.Empty;
		public Guid Token {
			get { return token; }
			set {
				if (value.Equals (token))
					return;
				token = value;
				OnPropertyChanged ("Token");
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

		private string login = string.Empty;
		public string Login {
			get { return login; }
			set {
				if (value.Equals (login))
					return;
				login = value;
				OnPropertyChanged ("Login");
			}
		}

		private string password = string.Empty;
		public string Password {
			get { return password; }
			set {
				if (value.Equals (password))
					return;
				password = value;
				OnPropertyChanged ("Password");
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

		private string email = string.Empty;
		public string Email {
			get { return email; }
			set {
				if (value != null && value.Equals (email))
					return;
				email = value;
				OnPropertyChanged ("Email");
			}
		}

		private string pushApple = string.Empty;
		public string PushApple {
			get { return pushApple; }
			set {
				if (value.Equals (pushApple))
					return;
				pushApple = value;
				OnPropertyChanged ("PushApple");
			}
		}

		private string pushGoogle = string.Empty;
		public string PushGoogle {
			get { return pushGoogle; }
			set {
				if (value.Equals (pushGoogle))
					return;
				pushGoogle = value;
				OnPropertyChanged ("PushGoogle");
			}
		}

		private string pushWindows = string.Empty;
		public string PushWindows {
			get { return pushWindows; }
			set {
				if (value.Equals (pushWindows))
					return;
				pushWindows = value;
				OnPropertyChanged ("PushWindows");
			}
		}

		private DateTime create = DateTime.UtcNow;
		public DateTime Create {
			get { return create; }
			set {
				if (value.Equals (create))
					return;
				create = value;
				OnPropertyChanged ("Create");
			}
		}

		private DateTime lastConnected = new DateTime (1999, 1, 1);
		public DateTime LastConnected {
			get { return lastConnected; }
			set {
				if (value.Equals (lastConnected))
					return;
				lastConnected = value;
				OnPropertyChanged ("LastConnected");
			}
		}

		private int countLogin = 0;
		public int CountLogin {
			get { return countLogin; }
			set {
				if (value.Equals (countLogin))
					return;
				countLogin = value;
				OnPropertyChanged ("CountLogin");
			}
		}

		private bool isModo = false;
		public bool IsModo {
			get { return isModo; }
			set {
				if (value.Equals (isModo))
					return;
				isModo = value;
				OnPropertyChanged ("IsModo");
			}
		}

		private bool isAdmin = false;
		public bool IsAdmin {
			get { return isAdmin; }
			set {
				if (value.Equals (isAdmin))
					return;
				isAdmin = value;
				OnPropertyChanged ("IsAdmin");
			}
		}


		[IgnoreDataMember]
		public bool IsBuilder { get { return idBuilder != Guid.Empty; } }

		public int CompareTo (User other) {
			return -other.NickName.CompareTo (this.NickName);
		}

	}
}