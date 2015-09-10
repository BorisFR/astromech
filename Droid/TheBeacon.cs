using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;

namespace AstroBuilders.Droid
{
	public static class TheBeacon
	{

		public static event FoundedBeacons Founded;

		private static MonitorNotifier _monitorNotifier;
		private static RangeNotifier _rangeNotifier;
		private static BeaconManager _beaconManager;

		private static List<Region> beaconRegion = new List<Region>();

		private static Dictionary<string, string> _info = new Dictionary<string, string>();

		public static void CheckInit(Dictionary<string, string> info) {
			_monitorNotifier = new MonitorNotifier();
			_rangeNotifier = new RangeNotifier();
			_info = info;
		}

		private static BeaconManager InitializeBeaconManager()
		{
			// Enable the BeaconManager 
			BeaconManager bm = BeaconManager.GetInstanceForApplication(Xamarin.Forms.Forms.Context);

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			bm.BeaconParsers.Add(iBeaconParser);

			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;
			_monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

			foreach (KeyValuePair<string, string> kvp in _info) {
				beaconRegion.Add (new AltBeaconOrg.BoundBeacon.Region (kvp.Key, Identifier.Parse (kvp.Value), null, null));
			}

			bm.SetBackgroundMode(false);
			bm.Bind((IBeaconConsumer)Xamarin.Forms.Forms.Context);

			return bm;
		}

		public static BeaconManager BeaconManagerImpl
		{
			get {
				if (_beaconManager == null)
				{
					_beaconManager = InitializeBeaconManager();
				}
				return _beaconManager;
			}
		}

		public static void StartMonitoring()
		{
			BeaconManagerImpl.SetForegroundBetweenScanPeriod(5000); // 5000 milliseconds = 5 secondes

			BeaconManagerImpl.SetMonitorNotifier(_monitorNotifier); 
			foreach (var region in beaconRegion) {
				_beaconManager.StartMonitoringBeaconsInRegion (region);
			}
		}

		public static void StartRanging()
		{
			BeaconManagerImpl.SetForegroundBetweenScanPeriod(500); // 500 milliseconds = 1/2 second

			BeaconManagerImpl.SetRangeNotifier(_rangeNotifier);
			foreach (var region in beaconRegion) {
				_beaconManager.StartRangingBeaconsInRegion(region);
			}
		}

		private static void DeterminedStateForRegionComplete(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("DeterminedStateForRegionComplete: " + e.Region + "=" + e.State);
		}

		private static void ExitedRegion(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("ExitedRegion: " + e.Region);
		}

		private static void EnteredRegion(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("EnteredRegion: " + e.Region);
		}

		private static Dictionary<string, OneBeacon> newBeacons = new Dictionary<string, OneBeacon> (100);

		private static async void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			//_data.Clear ();

			//var allBeacons = new List<Beacon> ();
			newBeacons.Clear ();
			foreach (var b in e.Beacons) {
				//allBeacons.Add (b);
				//System.Diagnostics.Debug.WriteLine (string.Format ("Beacon: {0} / {1} - {2}.{3} = {4:N0} cm", b.BluetoothName, b.Id1, b.Id2, b.Id3, b.Distance * 100));
				string id = string.Format ("{0}.{1}", b.Id2, b.Id3);
				OneBeacon ob = new OneBeacon ();
				ob.Id = id;
				ob.Distance = Convert.ToInt64 (b.Distance * 100.0);
				// par rapport à iOS
				// on a pas la proximity immediate/near/far
				// on simule en fonction de la distance
				if (ob.Distance < 30)
					ob.Proximity = 1;
				else if (ob.Distance < 100)
					ob.Proximity = 5;
				else
					ob.Proximity = 10;
				newBeacons.Add (id, ob);
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