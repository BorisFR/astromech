﻿using System;
using System.Collections.Generic;

namespace AstroBuildersModel
{
	public class Exhibition : IModel, IComparable<Exhibition>
	{

		private Guid idCountry = Guid.Empty;
		public Guid IdCountry {
			get { return idCountry; }
			set {
				if (value.Equals (idCountry))
					return;
				idCountry = value;
				OnPropertyChanged ("IdCountry");
			}
		}

		private DateTime startDate;
		public DateTime StartDate {
			get { return startDate; }
			set {
				if (value.Equals (startDate))
					return;
				startDate = value;
				OnPropertyChanged ("StartDate");
			}
		}

		private DateTime endDate;
		public DateTime EndDate {
			get { return endDate; }
			set {
				if (value.Equals (endDate))
					return;
				endDate = value;
				OnPropertyChanged ("EndDate");
			}
		}

		private string description = string.Empty;
		public string Description {
			get { return description; }
			set {
				if (value.Equals (description))
					return;
				description = value;
				OnPropertyChanged ("Description");
			}
		}

		private byte[] logo = new byte[0];
		public byte[] Logo {
			get { return logo; }
			set {
				if (value.Equals (logo))
					return;
				logo = value;
				OnPropertyChanged ("Logo");
			}
		}

		private byte[] flyer = new byte[0];
		public byte[] Flyer {
			get { return flyer; }
			set {
				if (value.Equals (flyer))
					return;
				flyer = value;
				OnPropertyChanged ("Flyer");
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


		private List<Guid> builders = new List<Guid>();
		public List<Guid> Builders {
			get { return builders; }
			set {
				if (value.Equals (builders))
					return;
				builders = value;
				OnPropertyChanged ("Builders");
			}
		}

		public int CompareTo (Exhibition other) {
			return -other.Title.CompareTo (this.Title);
		}

	}
}