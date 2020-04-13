using MVVMNavigation.Interfaces;
using Xamarin.Forms;

namespace MVVMNavigation.HistoryStack
{
	public class HistoryStackPage
	{
		private readonly Page _mainPage;

		public HistoryStackPage(Page mainPage)
		{
			_mainPage = mainPage;
		}

		public int Index(string pageName)
		{
			int index;
			Page(pageName, out index);
			return index;
		}

		public IViewModel ViewModel(string pageName)
		{
			IViewModel foundViewModel = null;
			int index;
			var foundPage = Page(pageName, out index);
			if (foundPage != null)
			{
				foundViewModel = foundPage.BindingContext as IViewModel;
			}

			return foundViewModel;
		}

		private Page Page(string pageName, out int index)
		{
			index = -1;
			int counter = 0;
			Page foundPage = null;
			var historyStack = _mainPage.Navigation.NavigationStack;

			for (int i = historyStack.Count - 1; i >= 0; i--)
			{
				var page = historyStack[i];

				var masterDetailPage = page as MasterDetailPage;

				if (masterDetailPage != null)
				{
					page = masterDetailPage.Detail;
				}

				if (page is NavigationPage)
				{
					page = (page as NavigationPage).CurrentPage;
				}

				if (page.GetType().Name == pageName)
				{
					index = counter;
					foundPage = page;
					break;
				}

				counter++;
			}

			return foundPage;
		}
	}
}
