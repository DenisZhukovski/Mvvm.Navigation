using MVVMNavigation.Interfaces;
using Xamarin.Forms;

namespace MVVMNavigation.Pages
{
	internal class MasterDetailPage : Xamarin.Forms.MasterDetailPage
	{
		public MasterDetailPage(Page menuPage, Page detailsPage, bool isPresented)
		{
			BackgroundColor = Color.White;
			IsPresented = isPresented;
			Master = menuPage;
			Detail = detailsPage;
			NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, string.Empty);
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == IsPresentedProperty.PropertyName)
			{
				var viewModel = Master.BindingContext as ISideMenuViewModel;
				if (viewModel != null)
				{
					viewModel.IsVisible = IsPresented;
				}
			}
		}
	}
}

