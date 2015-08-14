using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace AstroBuildersModel
{

	public delegate void SaveFile(string folder, string name, string data);

	public class IManager<T>
	{
		private string folderName = string.Empty;

		public event SaveFile SaveFile;

		public ObservableCollection<T> Collection { get; set; }

		private string collectionName;

		public string CollectionName {
			get { return collectionName; }
		}


		private string jsonData;

		public string JsonData {
			get { return jsonData; }
		}

		public IManager(string name) {
			collectionName = name;
			Collection = new ObservableCollection<T> ();
			folderName = name.Substring (0, name.IndexOf ("."));
		}

		public string FolderName { get { return folderName; } }

		private List<T> all = null;

		public List<T> All {
			get {
				if (all != null)
					return all;
				all = new List<T> (Collection);
				return all;
			}
			set { all = value; }
		}

		/*
		public string Save() {
			string json = JsonConvert.SerializeObject (Collection);
			return json;
		}
*/
		public void LoadFromJson(string json) {
			if (json == null)
				return;
			jsonData = json;
			if (json.Length < 1)
				return;
			Collection = JsonConvert.DeserializeObject<ObservableCollection<T>> (json);
			ReOrder ();
		}


		public void AddFromJSon(string json) {
			Collection.Add (JsonConvert.DeserializeObject<T> (json));
		}

		public void ReOrder() {
			all = null;
			// object <T> must be IComparable !!!
			List<T> sorted = Collection.OrderBy(x => x).ToList();
			for (int i = 0; i < sorted.Count(); i++)
				Collection.Move(Collection.IndexOf(sorted[i]), i);
		}

		public object GetByGuid<T>(Guid Guid) {
			foreach(var o in Collection)
				if((o as IModel).Id.Equals(Guid))
					return o;
			return null;
		}

		public void Add(T element) {
			IModel elt = element as IModel;
			if (elt.Id == Guid.Empty) {
				elt.Id = Guid.NewGuid ();
			}
			if (SaveFile != null)
				SaveFile (folderName, elt.Id.ToString (), JsonConvert.SerializeObject (element));
			Collection.Add (element);
			all = null;
		}

		public void Update(T element) {
			IModel elt = element as IModel;
			if (SaveFile != null)
				SaveFile (folderName, elt.Id.ToString (), JsonConvert.SerializeObject (element));
		}

	}
}