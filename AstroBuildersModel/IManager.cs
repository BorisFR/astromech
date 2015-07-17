using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace AstroBuildersModel
{

	public class IManager<T>
	{

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
		}

		private List<T> all;
		public List<T> All {
			get {
				if (all != null)
					return all;
				all = new List<T> (Collection);
				return all;
			}
			set { all = value; }
		}

		public string Save() {
			string json = JsonConvert.SerializeObject (Collection);
			return json;
		}

		public void LoadFromJson(string json) {
			if (json == null)
				return;
			jsonData = json;
			if (json.Length < 1)
				return;
			Collection = JsonConvert.DeserializeObject<ObservableCollection<T>> (json);
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
			if (elt.Id == Guid.Empty)
				elt.Id = Guid.NewGuid ();
			Collection.Add (element);
			all = null;
		}

	}
}