
using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SectionedUITableViewSample
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
			
			TableViewDataSource tvdc = new TableViewDataSource();
			this.tvc.TableView.DataSource = tvdc;
			
			this.searchBar.Delegate = new SearchDelegate(tvc, tvdc);
			
			window.AddSubview(this.tvc.View);
			
			window.MakeKeyAndVisible ();

			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
	public class SearchDelegate : UISearchBarDelegate
	{
		public UITableViewController tvc;
		public TableViewDataSource tvdc;
		public SearchDelegate(UITableViewController tvc, TableViewDataSource tvdc) : base ()
		{
			this.tvc = tvc;
			this.tvdc = tvdc;
		}
		
		public override void TextChanged (UISearchBar searchBar, string searchText)
		{
			tvdc.Search(searchText);
			tvc.TableView.DataSource = tvdc;
			tvc.TableView.ReloadData();
		}

	}
	
	public class TableViewDataSource : UITableViewDataSource
	{	
		private List<string> sectionList;
		private List<string> filteredSectionList = new List<string>();
		private List<string> dictionaryItems = new List<string>();
		public List<string> filteredDictionaryItems = new List<string>();
		public TableViewDataSource ()
		{
			Initialize();
		}
		
		public void Initialize()
		{
			sectionList = new List<string> {"A","B","C","D","E","F","G","H","J","I","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
			filteredSectionList = new List<string> {"A","B","C","D","E","F","G","H","J","I","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
		
			var random = new System.Random();
			
			for(int i = 0; i < 2000; i++)
			{
				dictionaryItems.Add(sectionList[random.Next(26)] + sectionList[random.Next(26)] + sectionList[random.Next(26)] + sectionList[random.Next(26)]);
			}
			dictionaryItems.Sort();
			foreach(var item in dictionaryItems)
			{
				filteredDictionaryItems.Add(item);	
			}
		}
		
		public void Search(string searchText)
		{
			filteredDictionaryItems.Clear();
			filteredSectionList.Clear();
			foreach(string item in dictionaryItems)
			{
				if(item.ToLower().Contains(searchText.ToLower()))
				{
					filteredDictionaryItems.Add(item);
					if(filteredSectionList.Find(x => x == item[0].ToString()) == null)
					{
						filteredSectionList.Add(item[0].ToString());	
					}
				}
			}
			filteredSectionList.Sort();
		}
		
		
		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return filteredSectionList.ToArray();
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return filteredSectionList.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return filteredSectionList[section];
		}
		
		public override int RowsInSection (MonoTouch.UIKit.UITableView tableview, int section)
		{
			
			return filteredDictionaryItems.FindAll(x => x.StartsWith(filteredSectionList[section])).Count;
		}
		
		public override UITableViewCell GetCell (MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var termsinsection = filteredDictionaryItems.FindAll(x => x.StartsWith(filteredSectionList[indexPath.Section]));
			
			string dictionaryItem = termsinsection[indexPath.Row];
			string cellID = "CellID";
			
			UITableViewCell cell = tableView.DequeueReusableCell(cellID);
			if(cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}
			cell.TextLabel.Text = dictionaryItem;
			return cell;
		}

		public override int SectionFor (UITableView tableView, string title, int atIndex)
		{
			return atIndex;
		}

	}
}
