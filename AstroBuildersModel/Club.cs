using System;

namespace AstroBuildersModel
{
	public class Club: IModel, IComparable<Club>
	{

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

		public int CompareTo (Club other) {
			return -other.Title.CompareTo (this.Title);
		}
	}
}