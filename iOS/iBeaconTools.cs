using System;
using AstroBuilders.iOS;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency (typeof (iBeaconTools))]

namespace AstroBuilders.iOS
{
	public class iBeaconTools : IBeaconTools
	{
		event FoundedBeacons Founded;

		object objectLock = new Object ();

		event FoundedBeacons IBeaconTools.Founded {
			add { lock (objectLock) {
					Founded += value;
				} }
			remove { lock (objectLock) {
					Founded -= value;
				} }
		}

		public void Init (Dictionary<string, string> info) {
			TheBeacon.Founded += HandleFounded;
			TheBeacon.CheckInit (info);
		}

		void HandleFounded (List<OneBeacon> beacons) {
				Founded (beacons);
		}

	}
}