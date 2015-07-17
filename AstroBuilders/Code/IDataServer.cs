using System;

namespace AstroBuilders
{

	public delegate void JobDone(bool status);

	public class IDataServer
	{
		public bool ForceFreshData = false;
		public event JobDone DataRefresh;
		public event JobDone JobDone;
		private string fileName = string.Empty;
		public string FileName { get { return fileName; } }

		public string JsonData = string.Empty;

		public IDataServer (string name)
		{
			fileName = name;
		}

		public IDataServer (string name, bool forceFreshData)
		{
			fileName = name;
			ForceFreshData = forceFreshData;
		}

		public void DoDownload() {
			// if exist, give the old data
			if (!ForceFreshData && Global.Files.IsExit (fileName)) {
				System.Diagnostics.Debug.WriteLine ("Read data from file: " + fileName);
				JsonData = Global.Files.ReadFile (fileName);
				if (JobDone != null)
					JobDone (true);
				//TriggerData (true);
				return;
			}
			// load newest data in background
			Tools.JobDone += Tools_JobDone;
			Tools.DoDownload (fileName);
		}

		void Tools_JobDone (bool status)
		{
			Tools.JobDone -= Tools_JobDone;
			JsonData = Tools.Result;
			if (status) {
				Global.Files.SaveFile (fileName, JsonData);
			}
			if (JobDone != null)
				JobDone (Tools.StatusDownload);
		}

		public void TriggerData(bool status) {
			if (DataRefresh != null)
				DataRefresh (status);
		}

	}
}