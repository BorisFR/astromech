using System;
using System.Collections.Generic;

namespace AstroBuilders
{
	public class OneBeacon
	{
		public string Id {get;set;}
		public long Proximity {get;set;}
		public long Distance {get;set;}

	}

	public delegate void FoundedBeacons(List<OneBeacon> beacons);

	public interface IBeaconTools
	{
		event FoundedBeacons Founded;

		void Init (Dictionary<string, string> info);
	}

}