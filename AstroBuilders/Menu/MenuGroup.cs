using System;
using System.Collections.ObjectModel;

namespace AstroBuilders
{
	public class MenuGroup : ObservableCollection<Menu>
	{

		public MenuGroup(string title) {
			Title = title;
		}

		public string Title { get; private set; }

	}
}