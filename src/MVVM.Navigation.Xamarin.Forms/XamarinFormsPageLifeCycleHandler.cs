using MVVM.Navigation.Xamarin.Forms.Interfaces;
using MVVMNavigation.Interfaces;
using Xamarin.Forms;
using MasterDetailPage = MVVMNavigation.Pages.MasterDetailPage;
using XamarinBindableObject = Xamarin.Forms.BindableObject;

namespace MVVM.Navigation.Xamarin.Forms
{
	public class XamarinFormsPageLifeCycleHandler : IPageLifeCycleHandler
	{
		public virtual void OnCreated(Page page, Page prevPage, IViewModel viewModel)
		{
			Bind(page, viewModel);
		    // ReSharper disable once SuspiciousTypeConversion.Global
		    if (viewModel is IHaveSideMenu viewModelWithMenu)
			{
				Bind((MasterDetailPage)page, viewModel, viewModelWithMenu.MenuViewModel);
			}

			TryToSetSecondaryMenu(page, viewModel);
		}

		public virtual void WillAppear(Page page)
		{
		    if (page is IVisibilityControlledPage visibilityControlledPage && !visibilityControlledPage.IsVisibleOnTheScreen)
			{
				visibilityControlledPage.ViewWillAppear();
			}
		}

		public virtual void WillDisappear(Page page)
		{
		    if (page is IVisibilityControlledPage visibilityControlledPage && visibilityControlledPage.IsVisibleOnTheScreen)
			{
				visibilityControlledPage.ViewWillDisappear();
			}
		}

		public virtual void DidEnterBackground(Page page)
		{
		    if (page is IVisibilityControlledPage visibilityControlledPage)
			{
				visibilityControlledPage.DidEnterBackground();
			}
		}

		protected virtual void TryToSetSecondaryMenu(Page page, IViewModel viewModel)
		{
		    // ReSharper disable once SuspiciousTypeConversion.Global
		    if (viewModel is IHaveSecondaryMenu secondaryMenuViewModel)
			{
				foreach (var item in secondaryMenuViewModel.MenuViewModel.Items)
				{
					page.ToolbarItems.Add(new ToolbarItem
					{
						Text = item,
						Order = ToolbarItemOrder.Secondary,
						Command = secondaryMenuViewModel.MenuViewModel.SelectCommand,
						CommandParameter = item
					});
				}
			}
		}

		private void Bind(XamarinBindableObject bindableObject, IViewModel viewModel)
		{
			bindableObject.BindingContext = viewModel;
		}

		private void Bind(MasterDetailPage masterDetailPage, IViewModel viewModel, ISideMenuViewModel menuViewModel)
		{
			Bind(masterDetailPage.Detail, viewModel);
			Bind(masterDetailPage.Master, menuViewModel);

			menuViewModel.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
				{
					masterDetailPage.IsPresented = menuViewModel.IsVisible;
				}
			};
		}
	}
}
