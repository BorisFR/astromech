using System;
using System.Runtime.Serialization;

namespace AstroBuildersModel
{
	public class Country : IModel, IComparable<Country>
	{

		private string code = string.Empty;
		public string Code {
			get { return code; }
			set {
				if (value.Equals (code))
					return;
				code = value;
				OnPropertyChanged ("Code");
			}
		}

		[IgnoreDataMember]
		public string Flag {
			get {
				return string.Format ("Content/Country/{0}.png", code);
			}
		}

		public int CompareTo (Country other) {
			return -other.code.CompareTo (this.code);
		}

	}
}
