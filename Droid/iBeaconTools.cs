﻿using System;
using AstroBuilders.Droid;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency (typeof (iBeaconTools))]

namespace AstroBuilders.Droid
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