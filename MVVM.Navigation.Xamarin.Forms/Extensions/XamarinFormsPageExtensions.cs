using Xamarin.Forms;
using MasterDetailPage = Xamarin.Forms.MasterDetailPage;

namespace MVVM.Navigation.Xamarin.Forms.Extensions
{
	public static class XamarinFormsPageExtensions
	{
		public static Page ContentPage(this Page page)
		{
		    if (page is ContentPage contentPage)
			{
				return contentPage;
			}

		    if (page is MasterDetailPage rootPage)
			{
				return ContentPage(rootPage.Detail);
			}

		    if (page is NavigationPage navPage)
			{
				return ContentPage(navPage.CurrentPage);
			}

			var tabbedPage = page as TabbedPage;
			return tabbedPage;
		}

		public static Page NavigationPage(this Page page)
		{
			if (!(page is NavigationPage) && page is MasterDetailPage)
			{
				return ((MasterDetailPage)page).Detail.NavigationPage();
			}

			return page;
		}
	}
}
