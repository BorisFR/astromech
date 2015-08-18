using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class ViewAdminBuilders : ContentView
	{

		User user = null;
		Builder builder = null;
		bool isAutoselect = false;

		public ViewAdminBuilders ()
		{
			InitializeComponent ();

			theBuilders.ItemsSource = Global.AllBuilders.Collection;

			IDataServer x = new IDataServer ("users", true, true);
			x.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + x.FileName + "=" + status);
				if(!status)
					return;
				Global.AllUsers.LoadFromJson(Helper.Decrypt(result));
				Device.BeginInvokeOnMainThread (() => {
					theUsers.ItemsSource = Global.AllUsers.Collection;
				});
			};
			DataServer.AddToDo (x);

			ShowButtons ();

			theUsers.ItemSelected += TheUsers_ItemSelected;
			theBuilders.ItemSelected += TheBuilders_ItemSelected;

			btIsBuilder.Clicked += BtIsBuilder_Clicked;
			btIsNotBuilder.Clicked += BtIsNotBuilder_Clicked;
			btCreateBuilder.Clicked += BtCreateBuilder_Clicked;

		}

		void BtCreateBuilder_Clicked (object sender, EventArgs e)
		{
			// TODO: create builder
		}

		void BtIsNotBuilder_Clicked (object sender, EventArgs e)
		{
			btIsBuilder.IsEnabled = false;
			user.IdBuilder = Guid.Empty;
			Tools.JobDone += Tools_LinkDone;
			Tools.DoUpdateUserByAdmin (user);
		}

		void BtIsBuilder_Clicked (object sender, EventArgs e)
		{
			btIsBuilder.IsEnabled = false;
			user.IdBuilder = builder.Id;
			Tools.JobDone += Tools_LinkDone;
			Tools.DoUpdateUserByAdmin (user);

		}

		void Tools_LinkDone (bool status, string result) {
			Tools.JobDone -= Tools_LinkDone;
			if (!status) {
				ShowButtons ();
				return;
			}
			theUsers.SelectedItem = null;
			theBuilders.SelectedItem = null;
			try {
				string json = Helper.Decrypt (result); //Tools.Result);
				Global.AllUsers.LoadFromJson (Helper.Decrypt (json));
				Device.BeginInvokeOnMainThread (() => {
					theUsers.ItemsSource = Global.AllUsers.Collection;
				});
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("Error: " + err.Message);
			}
			ShowButtons ();
		}

		void TheBuilders_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				isAutoselect = false;
				builder = null;
				ShowButtons ();
				return;
			}
			builder = (Builder)e.SelectedItem;
			foreach (User u in Global.AllUsers.Collection) {
				if (u.IsBuilder && u.IdBuilder == builder.Id) {
					theUsers.SelectedItem = u;
					isAutoselect = true;
					theUsers.ScrollTo (u, ScrollToPosition.Center, true);
					break;
				}
			}
			isAutoselect = false;
			if (theUsers.SelectedItem != null) {
				user = (User)theUsers.SelectedItem;
				if (user.IsBuilder) {
					if (user.IdBuilder != builder.Id) {
						theUsers.SelectedItem = null;
						user = null;
					} else {
						isAutoselect = true;
					}
				}
			}
			ShowButtons ();
		}

		void TheUsers_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				isAutoselect = false;
				user = null;
				theBuilders.IsEnabled = true;
				ShowButtons ();
				return;
			}
			user = (User)e.SelectedItem;
			if (user.IsBuilder) {
				isAutoselect = true;
				builder = (Builder)Global.AllBuilders.GetByGuid<Builder> (user.IdBuilder);
				theBuilders.SelectedItem = builder;
				//theBuilders.IsEnabled = false;
				theBuilders.ScrollTo (builder, ScrollToPosition.Center, true);
			} else {
				isAutoselect = false;
				if (builder != null) {
					foreach (User u in Global.AllUsers.Collection) {
						if (u.IsBuilder && u.IdBuilder == builder.Id) {
							theBuilders.SelectedItem = null;
							builder = null;
							break;
						}
					}
				}
				theBuilders.IsEnabled = true;
			}
			ShowButtons ();
		}

		private void ShowButtons() {
			if (user == null || builder == null) {
				btCreateBuilder.IsEnabled = false;
				btIsBuilder.IsEnabled = false;
				btIsNotBuilder.IsEnabled = false;
			}
			if (user != null && builder == null) {
				btCreateBuilder.IsEnabled = true;
			}
			if (user != null && builder != null) {
				if (isAutoselect) {
					btCreateBuilder.IsEnabled = false;
					btIsBuilder.IsEnabled = false;
					btIsNotBuilder.IsEnabled = true;
				} else {
					btCreateBuilder.IsEnabled = false;
					btIsBuilder.IsEnabled = true;
					btIsNotBuilder.IsEnabled = false;
				}
			}
		}

	}
}