﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace AstroBuildersModel
{
	public class IModel : INotifyPropertyChanged
	{
		public Guid Id { get; set; }

		private string title = string.Empty;
		public string Title {
			get { return title; }
			set {
				if (value.Equals (title))
					return;
				title = value;
				OnPropertyChanged ("Title");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged (string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) {
				handler (this, new PropertyChangedEventArgs (propertyName));
			}
		}

		[IgnoreDataMember]
		public string JSonData {
			get{ return JsonConvert.SerializeObject (this); }
		}

	}
}