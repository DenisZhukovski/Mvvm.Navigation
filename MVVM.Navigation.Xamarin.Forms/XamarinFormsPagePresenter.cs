using System;
using MVVM.Navigation.Xamarin.Forms.Interfaces;
using MVVMNavigation.Interfaces;
using Xamarin.Forms;

namespace MVVM.Navigation.Xamarin.Forms
{
	public class XamarinFormsPagePresenter : IPagePresenter
	{
		private readonly IPageResolver _pageResolver;

		public XamarinFormsPagePresenter(IPageResolver pageResolver)
		{
			_pageResolver = pageResolver;
		}

		public Page Page<TViewModel>() where TViewModel : IViewModel
		{
			return _pageResolver.Page(PageName<TViewModel>());
		}

		public Page Page(Type viewModelType)
		{
			return _pageResolver.Page(PageName(viewModelType));
		}

		public Page Page<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
		{
		    // ReSharper disable once SuspiciousTypeConversion.Global
			if (viewModel is IHaveSideMenu)
			{
				return PageWithMenu(viewModel);
			}

			Page page = Page(viewModel.GetType());
			return page;
		}

		public string PageName<TViewModel>() where TViewModel : IViewModel
		{
			return PageName(typeof(TViewModel));
		}

		public string PageName(Type viewModelType)
		{
			return viewModelType.Name.Replace("ViewModel", "Page");
		}

		public virtual NavigationPage NavigationPage(Page page)
		{
			return new NavigationPage(page);
		}

		protected virtual MasterDetailPage MasterDetailPage(Page menuPage, Page detailsPage)
		{
			return new MVVMNavigation.Pages.MasterDetailPage(menuPage, NavigationPage(detailsPage), false);
		}

		private Page PageWithMenu(IViewModel viewModel)
		{
		    // ReSharper disable once SuspiciousTypeConversion.Global
			var viewModelWithMenu = (IHaveSideMenu)viewModel;
			Page menuPage = Page(viewModelWithMenu.MenuViewModel);
			Page detailsPage = Page(viewModel.GetType());
			return MasterDetailPage(menuPage, detailsPage);
		}
	}
}