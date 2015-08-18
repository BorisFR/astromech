using System;

namespace AstroBuilders
{

	public delegate void JobDone(bool status, string result);

	public class IDataServer
	{
		public bool IgnoreLocalData = false;
		public bool ForceFreshData = false;
		public event JobDone DataRefresh;
		public event JobDone JobDone;
		private string fileName = string.Empty;
		public string FileName { get { return fileName; } }

		//public string JsonData = string.Empty;

		public IDataServer (string name)
		{
			fileName = name;
		}

		public IDataServer (string name, bool forceFreshData)
		{
			fileName = name;
			ForceFreshData = forceFreshData;
		}

		public IDataServer (string name, bool forceFreshData, bool ignoreLocalData)
		{
			fileName = name;
			ForceFreshData = forceFreshData;
			IgnoreLocalData = ignoreLocalData;
		}

		public bool HasOldData {
			get {
				if (IgnoreLocalData)
					return false;
				return Global.Files.IsExit (fileName);
			}
		}

		public string OldData {
			get {
				System.Diagnostics.Debug.WriteLine ("Read data from file: " + fileName);
				return Global.Files.ReadFile (fileName);
			}
		}

		public void DoDownload() {
			/*
			// if exist, give the old data
			//if (Global.Files.IsExit (fileName)) {
			if (!ForceFreshData && Global.Files.IsExit (fileName)) {
				System.Diagnostics.Debug.WriteLine ("Read data from file: " + fileName);
				string jsonData = Global.Files.ReadFile (fileName);
				JobDone (true, jsonData);
				//TriggerData (true);
				//if (!ForceFreshData)
					return;
			}
			*/
			// load newest data in background
			Tools.DoneBatch += DoneBatch;
			Tools.DoDownload (fileName);
		}

		void DoneBatch (bool status, string result)
		{
			Tools.DoneBatch -= DoneBatch;
			//JsonData = result; //Tools.Result;
			if (status) {
				Global.Files.SaveFile (fileName, result);
			}
			JobDone (status, result);
			//if (DataRefresh != null)
			//	DataRefresh (status, result);
		}

		public void TriggerData(bool status, string result) {
			DataRefresh (status, result);
		}

	}
}