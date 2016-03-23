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

		private static PeripheralManagerDelegate peripheralDelegate;
		private static CBPeripheralManager peripheralManager;

		private static CLLocationManager locationMgr = new CLLocationManager ();
		private static List<CLBeaconRegion> beaconRegion = new List<CLBeaconRegion> ();

		private static Dictionary<string, string> _info = new Dictionary<string, string> ();

		public static void CheckInit (Dictionary<string, string> info)
		{
			if (peripheralManager != null)
				return;

			_info = info;

			NSUuid id;
			int i = 0;
			foreach (KeyValuePair<string, string> kvp in _info) {
				try {
					id = new NSUuid (kvp.Value); // beaconId);
				} catch (Exception err) {
					Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, "iBeacon", "iBeacons UUID is wrong!");
					return;
				}
				beaconRegion.Add (new CLBeaconRegion (id, kvp.Key)); //regionName);
			}
			peripheralDelegate = new PeripheralManagerDelegate ();
			peripheralDelegate.StateUpdatedEvent += PeripheralDelegate_StateUpdatedEvent;

			peripheralManager = new CBPeripheralManager (peripheralDelegate, DispatchQueue.DefaultGlobalQueue, new NSDictionary ());
			//peripheralManager = new CBPeripheralManager (ICBPeripheralManagerDelegate, DispatchQueue.DefaultGlobalQueue, new NSDictionary());
			//peripheralManager.StateUpdated += HandleStateUpdated;
		}

		static void PeripheralDelegate_StateUpdatedEvent (CBPeripheralManager peripheral)
		{
			peripheralDelegate.StateUpdatedEvent -= PeripheralDelegate_StateUpdatedEvent;
			if (peripheralManager.State < CBPeripheralManagerState.PoweredOn) {
				Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, "iBeacon", "Bluetooth must be enabled.");
			} else {
				if (peripheralManager.State == CBPeripheralManagerState.PoweredOn) {
					StartMonitoring ();
				} else {
					Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, "iBeacon", "State: " + peripheralManager.State);
				}
			}
		}

		static void HandleStateUpdated (object sender, EventArgs e)
		{
			peripheralManager.StateUpdated -= HandleStateUpdated;
			if (peripheralManager.State < CBPeripheralManagerState.PoweredOn) {
				Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, "iBeacon", "Bluetooth must be enabled.");
			} else {
				if (peripheralManager.State == CBPeripheralManagerState.PoweredOn) {
					StartMonitoring ();
				}
			}
		}


		private static bool isMonitoring = false;

		public static void StartMonitoring ()
		{
			if (isMonitoring)
				return;
			isMonitoring = true;
			foreach (CLBeaconRegion region in beaconRegion) {
				region.NotifyEntryStateOnDisplay = true;
				region.NotifyOnEntry = true;
				region.NotifyOnExit = true;
			}
//			beaconRegion.NotifyEntryStateOnDisplay = true;
//			beaconRegion.NotifyOnEntry = true;
//			beaconRegion.NotifyOnExit = true;
			locationMgr.RequestWhenInUseAuthorization ();
			try {
				locationMgr.RequestAlwaysAuthorization ();
			} catch (Exception err) {
				Console.WriteLine ("ERROR: " + err.Message);
			}
			locationMgr.RegionEntered += HandleRegionEntered;
			locationMgr.RegionLeft += HandleRegionLeft;
			locationMgr.DidDetermineState += HandleDidDetermineState;
			locationMgr.DidStartMonitoringForRegion += HandleDidStartMonitoringForRegion;
			locationMgr.DidRangeBeacons += HandleDidRangeBeacons;
			//Console.WriteLine (string.Format("StartMonitoring {0}", beaconRegion.Identifier));
			foreach (CLBeaconRegion region in beaconRegion) {
				locationMgr.StartMonitoring (region);
				locationMgr.StartRangingBeacons (region);
			}
//			locationMgr.StartMonitoring (beaconRegion);
//			locationMgr.StartRangingBeacons (beaconRegion);
		}

		static void HandleDidStartMonitoringForRegion (object sender, CLRegionEventArgs e)
		{
			string t_region = e.Region.Identifier.ToString ();
			Console.WriteLine (t_region);
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
					Console.WriteLine ("region unknown");
					break;
				default:
					Console.WriteLine ("region state unknown");
					break;
			}
		}

		static void HandleRegionLeft (object sender, CLRegionEventArgs e)
		{
//			if (e.Region.Identifier == regionName){
//				Console.WriteLine("beacon region exited");
//			}
		}

		static void HandleRegionEntered (object sender, CLRegionEventArgs e)
		{
//			if (e.Region.Identifier == regionName) {
//				Console.WriteLine ("beacon region entered");
//			}
		}

		//private static Dictionary<string, OneBeacon> oldBeacons = new Dictionary<string, OneBeacon> (100);
		private static Dictionary<string, OneBeacon> newBeacons = new Dictionary<string, OneBeacon> (100);

		static void HandleDidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
		{
//			if (!e.Region.Identifier.Equals (regionName)) {
//				Console.WriteLine (string.Format ("Found Region: {0}", e.Region.Identifier));
//				return;
//			}
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
						double tmp2 = tmp1 * 100;
						v = Convert.ToInt64 (tmp2, CultureInfo.InvariantCulture);
					} catch (Exception) {
						v = Convert.ToInt64 (Convert.ToDouble (val.Replace (".", ",")) * 100);
					}
					string id = string.Format ("{0}.{1}", b.Major, b.Minor);
					OneBeacon ob = new OneBeacon ();
					ob.Id = id;
					ob.Distance = v;

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
				} catch (Exception err) {
					Console.WriteLine (err.Message);
				}
			}
			if (newBeacons.Count > 0) {
				List<OneBeacon> list = new List<OneBeacon> ();
				foreach (KeyValuePair<string,OneBeacon> kvp in newBeacons) {
					list.Add (kvp.Value);
				}
				// on envoi l'info
				Founded (list);
			}
		}

	}
}