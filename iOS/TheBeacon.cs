using System;
using System.Collections.Generic;
using System.Globalization;
using CoreBluetooth;
using CoreLocation;
using Foundation;
using CoreFoundation;

namespace AstroBuilders.iOS
{

	public static class TheBeacon
	{
		public static event FoundedBeacons Founded;

		private static string regionName = string.Empty;

		private static PeripheralManagerDelegate peripheralDelegate;
		private static CBPeripheralManager peripheralManager;

		private static CLLocationManager locationMgr = new CLLocationManager ();
		private static CLBeaconRegion beaconRegion ;

		public static void CheckInit(string region, string beaconId)
		{
			if (peripheralManager != null)
				return;

			regionName = region;

			NSUuid id;
			try {
				id = new NSUuid (beaconId);
			} catch (Exception err) {
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error ,"iBeacon", "iBeacons UUID is wrong!");
				return;
			}
			beaconRegion = new CLBeaconRegion (id, regionName);

			peripheralDelegate = new PeripheralManagerDelegate ();
			peripheralDelegate.StateUpdatedEvent += PeripheralDelegate_StateUpdatedEvent;

			peripheralManager = new CBPeripheralManager (peripheralDelegate, DispatchQueue.DefaultGlobalQueue, new NSDictionary());
			//peripheralManager = new CBPeripheralManager (ICBPeripheralManagerDelegate, DispatchQueue.DefaultGlobalQueue, new NSDictionary());
			//peripheralManager.StateUpdated += HandleStateUpdated;
		}

		static void PeripheralDelegate_StateUpdatedEvent (CBPeripheralManager peripheral)
		{
			peripheralDelegate.StateUpdatedEvent -= PeripheralDelegate_StateUpdatedEvent;
			if (peripheralManager.State < CBPeripheralManagerState.PoweredOn) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "iBeacon", "Bluetooth must be enabled.");
			} else {
				if (peripheralManager.State == CBPeripheralManagerState.PoweredOn) {
					StartMonitoring ();
				} else {
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "iBeacon", "State: " + peripheralManager.State);
				}
			}
		}

		static void HandleStateUpdated (object sender, EventArgs e)
		{
			peripheralManager.StateUpdated -= HandleStateUpdated;
			if (peripheralManager.State < CBPeripheralManagerState.PoweredOn) {
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error ,"iBeacon", "Bluetooth must be enabled.");
			} else {
				if (peripheralManager.State == CBPeripheralManagerState.PoweredOn) {
					StartMonitoring ();
				}
			}
		}


		private static bool isMonitoring = false;
		public static void StartMonitoring()
		{
			if (isMonitoring)
				return;
			isMonitoring = true;
			beaconRegion.NotifyEntryStateOnDisplay = true;
			beaconRegion.NotifyOnEntry = true;
			beaconRegion.NotifyOnExit = true;
			locationMgr.RequestWhenInUseAuthorization ();
			try{
			locationMgr.RequestAlwaysAuthorization ();
			}catch(Exception err) {
				Console.WriteLine ("ERROR: " + err.Message);
			}
			locationMgr.RegionEntered += HandleRegionEntered;
			locationMgr.RegionLeft += HandleRegionLeft;
			locationMgr.DidDetermineState += HandleDidDetermineState;
			locationMgr.DidStartMonitoringForRegion += HandleDidStartMonitoringForRegion;
			locationMgr.DidRangeBeacons += HandleDidRangeBeacons;
			//Console.WriteLine (string.Format("StartMonitoring {0}", beaconRegion.Identifier));
			locationMgr.StartMonitoring (beaconRegion);
			locationMgr.StartRangingBeacons (beaconRegion);
		}

		static void HandleDidStartMonitoringForRegion (object sender, CLRegionEventArgs e)
		{
			string t_region = e.Region.Identifier.ToString();
			Console.WriteLine(t_region);
		}

		static void HandleDidDetermineState (object sender, CLRegionStateDeterminedEventArgs e)
		{
			switch (e.State) {
			case CLRegionState.Inside:
				Console.WriteLine ("region state inside");
				break;
			case CLRegionState.Outside:
				Console.WriteLine ("region state outside");
				break;
			case CLRegionState.Unknown:
				Console.WriteLine("region unknown");
				break;
			default:
				Console.WriteLine ("region state unknown");
				break;
			}
		}

		static void HandleRegionLeft (object sender, CLRegionEventArgs e)
		{
			if (e.Region.Identifier == regionName){
				Console.WriteLine("beacon region exited");
			}
		}

		static void HandleRegionEntered (object sender, CLRegionEventArgs e)
		{
			if (e.Region.Identifier == regionName) {
				Console.WriteLine ("beacon region entered");
			}
		}

		private static Dictionary<string, OneBeacon> oldBeacons = new Dictionary<string, OneBeacon> (100);
		private static Dictionary<string, OneBeacon> newBeacons = new Dictionary<string, OneBeacon> (100);

		static void HandleDidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
		{
			if (!e.Region.Identifier.Equals (regionName)) {
				Console.WriteLine (string.Format ("Found Region: {0}", e.Region.Identifier));
				return;
			}
			newBeacons.Clear ();
			foreach (CLBeacon b in e.Beacons) {
				if (b.Proximity == CLProximity.Unknown)
					continue;
				try {
					// on extrait la distance en metre, que l'on converti en cm
					string s = b.ToString ();
					int start = s.IndexOf ("+/-");
					start += 3;
					int end = s.IndexOf ("m,", start);
					string val = s.Substring (start, end - start);
					long v = 0;
					try {
						double tmp1 = Convert.ToDouble (val, CultureInfo.InvariantCulture);
						double tmp2 = tmp1 *100;
						v = Convert.ToInt64 (tmp2, CultureInfo.InvariantCulture);
					} catch (Exception) {
						v = Convert.ToInt64 (Convert.ToDouble (val.Replace (".", ",")) * 100);
					}
					string id = string.Format ("{0}.{1}", b.Major, b.Minor);
					OneBeacon ob = new OneBeacon ();
					ob.Id = id;
					ob.Distance = v;
					if (v < 10) {
						int z = 1 + 2; // juste pour mettre un point d'arrêt en debug :)
					}
					// CLBeacon (uuid:<__NSConcreteUUID 0x146a51c0> B9407F30-F5F8-466E-AFF9-25556B57FE6D, major:26031, minor:22602, proximity:1 +/- 0.24m, rssi:-53)
					//					if (b.Major.ToString ().Equals ("26031") && b.Minor.ToString ().Equals ("22602")) {
					//						Console.WriteLine (b.Proximity.ToString () + " " + val);
					//					}
					//Console.WriteLine (string.Format ("{0}/{1}/{2} - {3}@{4} ", b.Major, b.Minor, b.Proximity, b.Accuracy, b.Rssi));

					/*
					if (oldBeacons.ContainsKey (id)) {
						// on connait déjà le beacon
						// threshold
						// on ne balance pas tout les changements, seulement les significatifs
						// c'est variable fonction de l'éloignement
						long threshold = Math.Abs (oldBeacons [id].Distance - v);
						switch (b.Proximity) {
						case CLProximity.Far:
							ob.Proximity = 10;
							if (threshold > 2) { // info si changement au metre car on est loin
								newBeacons.Add (id, ob);
							}
							break;
						case CLProximity.Near:
							ob.Proximity = 5;
							if (threshold > 2) { // info si changement d'au moins 50 cm
								newBeacons.Add (id, ob);
							}
							break;
						case CLProximity.Immediate:
							ob.Proximity = 1;
							if (threshold > 2) { // info si changement d'au moins 10 cm
								newBeacons.Add (id, ob);
							}
							break;
						}
						oldBeacons [id] = ob;
					} else {
					*/
					// nouveau beacon, on envoi l'info
					switch (b.Proximity) {
					case CLProximity.Far:
						ob.Proximity = 10;
						break;
					case CLProximity.Near:
						ob.Proximity = 5;
						break;
					case CLProximity.Immediate:
						ob.Proximity = 1;
						break;
					}							
					newBeacons.Add (id, ob);
					//oldBeacons.Add (id, ob);
					//}
					//}
					// matching avec ce que l'on connait
				} catch (Exception err) {
					Console.WriteLine (err.Message);
				}
			}
			// si on a des nouveaux beacons ou des beacons dont le changement est significatif
			if (newBeacons.Count > 0) {
				List<OneBeacon> list = new List<OneBeacon> ();
				//OneBeacon[] arr = new OneBeacon[newBeacons.Count];
				//int cpt = 0;
				foreach (KeyValuePair<string,OneBeacon> kvp in newBeacons) {
					//arr [cpt++] = kvp.Value;
					list.Add (kvp.Value);
				}
				// on envoi l'info
				Founded (list);
				//				Communication comm = new Communication ();
				//				comm.PushBeacons (arr);
			}
		}

		//SystemSound.Vibrate.PlayAlertSound ();
		//SystemSound.Vibrate.PlaySystemSound ();

	}
}