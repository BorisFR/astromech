using System;

namespace AstroBuildersModel
{
	public class Card : IModel, IComparable<Card>
	{

		private Guid idExhibition = Guid.Empty;
		public Guid IdExhibition {
			get { return idExhibition; }
			set {
				if (value.Equals (idExhibition))
					return;
				idExhibition = value;
				OnPropertyChanged ("IdExhibition");
			}
		}

		private string idBeacon = string.Empty;
		public string IdBeacon {
			get { return idBeacon; }
			set {
				if (value.Equals (idBeacon))
					return;
				idBeacon = value;
				OnPropertyChanged ("IdBeacon");
			}
		}

		private int distance = 100;
		public int Distance {
			get { return distance; }
			set {
				if (value.Equals (distance))
					return;
				distance = value;
				OnPropertyChanged ("Distance");
			}
		}

		private string qrCode = string.Empty;
		public string QrCode {
			get { return qrCode; }
			set {
				if (value.Equals (qrCode))
					return;
				qrCode = value;
				OnPropertyChanged ("QrCode");
			}
		}

		private byte[] theImage = new byte[0];
		public byte[] TheImage {
			get { return theImage; }
			set {
				if (value.Equals (theImage))
					return;
				theImage = value;
				OnPropertyChanged ("TheImage");
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

		private Guid idRobot = Guid.Empty;
		public Guid IdRobot {
			get { return idRobot; }
			set {
				if (value.Equals (idRobot))
					return;
				idRobot = value;
				OnPropertyChanged ("IdRobot");
			}
		}

		public int CompareTo (Card other) {
			return -other.Title.CompareTo (this.Title);
		}

	}
}