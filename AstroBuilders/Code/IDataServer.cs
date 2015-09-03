using System;
using System.Threading.Tasks;

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

        private bool isExit = false;
        private bool isTest = false;

        public async Task<bool> HasOldData()
        {
            if (IgnoreLocalData) {
                System.Diagnostics.Debug.WriteLine("Ignore local for: " + fileName);
                return false;
            }
            if (isTest)
                return isExit;
            isTest = true;
            System.Diagnostics.Debug.WriteLine("HasOldData testing fileExist: " + fileName);
            isExit = await Global.Files.IsExit(fileName);
            return isExit;
        }

        public async Task<string> OldData()
        {
            System.Diagnostics.Debug.WriteLine("Read data from file: " + fileName);
            string x = await Global.Files.ReadFile(fileName);
            return x;
        }

		public void DoDownload() {
			Tools.DoneBatch += DoneBatch;
			Tools.DoDownload (fileName);
		}

		async void DoneBatch (bool status, string result)
		{
			Tools.DoneBatch -= DoneBatch;
			//JsonData = result; //Tools.Result;
			if (status) {
				await Global.Files.SaveFile (fileName, result);
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