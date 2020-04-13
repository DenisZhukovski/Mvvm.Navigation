using System;
using System.Threading.Tasks;
using MVVMNavigation.Extensions;
using MVVMNavigation.Interfaces;

namespace MVVMNavigation.ViewModelLifeCycle
{
	public class ViewModelLifeCycleHandler : IViewModelLifeCycleHandler
	{
		/// <summary>
		/// This delay is usually used to handle side Menu control animation issue on iOs platform.
		/// It's about side menu hiding animation problem that occurs when we receive an exception on a target's view model.
		/// In that case warning alert that should be shown on the target's page will disappear.
		/// </summary>
		private const int AnimationsDelay = 1000;

		public virtual void InitWithParameter<TViewModelNavigationObject>(
			IViewModelWithParameter<TViewModelNavigationObject> viewModel,
			TViewModelNavigationObject navigationObject)
		{
			viewModel.ViewModelInit(navigationObject);
		}

		public virtual void OnCreated(IViewModel viewModel)
		{
			TryToSubscriveOnMenuEvents(viewModel);
		}

		public virtual async Task ViewModelWillAppear(IViewModel viewModel, bool needLoadData)
		{
			
			if (viewModel != null && !IsSideMenuViewModel(viewModel))
			{
				try
				{
					await viewModel.WillAppear(needLoadData);
					if (needLoadData)
					{
						await LoadViewModelData(viewModel);
					}
				}
				catch (Exception e)
				{
					if (
						!viewModel.HandleException(e, () => ViewModelWillAppear(viewModel, needLoadData).InBackground(),
							() => OnCancel(viewModel)))
					{
						throw;
					}
				}
			}
		}

		public virtual void ViewModelWillDisappear(IViewModel viewModel)
		{
			viewModel.WillDisappear();
		}

		public virtual void DidEnterBackground(IViewModel viewModel)
		{
		    viewModel?.BecameInvisible();
		}

		public virtual bool OnBackButtonPressed(IViewModel viewModel)
		{
		    if (viewModel is IHaveBackButton haveBackButton)
			{
				return haveBackButton.Back();
			}

			return false;
		}

		public virtual void OnAppearing(IViewModel viewModel)
		{
			if (viewModel != null && !IsSideMenuViewModel(viewModel))
			{
				try
				{
					OnNavigationComplete(viewModel);
					viewModel.BecameVisible();
				}
				catch (Exception e)
				{
					if (!viewModel.HandleException(e, () => OnAppearing(viewModel), () => OnCancel(viewModel)))
					{
						throw;
					}
				}
			}
		}

		public virtual void OnDisappearing(IViewModel viewModel)
		{
			if (viewModel != null && !IsSideMenuViewModel(viewModel))
			{
				try
				{
					viewModel.BecameInvisible();
				}
				catch (Exception e)
				{
					if (!viewModel.HandleException(e, () => OnDisappearing(viewModel), () => OnCancel(viewModel)))
					{
						throw;
					}
				}
			}
		}

		protected virtual async Task LoadViewModelData(IViewModel viewModel)
		{
			Exception exception = null;
			try
			{
				viewModel.IsLoading = true;
				viewModel.IsDataLoaded = false;
				await viewModel.LoadData();
				viewModel.IsDataLoaded = true;
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				if (exception == null)
				{
					viewModel.IsLoading = false;
				}
			}

			if (exception != null)
			{
				await Task.Delay(AnimationsDelay);
				viewModel.IsLoading = false;
				if (!viewModel.HandleException(exception, () => LoadViewModelData(viewModel).InBackground(), () => OnCancel(viewModel)))
				{
					throw exception;
				}
			}
		}

		private bool IsSideMenuViewModel(IViewModel viewModel)
		{
			return viewModel is ISideMenuViewModel;
		}

		private void OnNavigationComplete(IViewModel viewModel)
		{
			if (viewModel.IsNavigatable)
			{
				viewModel.Navigation.NavigationCompleted(viewModel.GetType());
			}
		}

		private void OnCancel(IViewModel viewModel)
		{
			if (!OnBackButtonPressed(viewModel))
			{
				viewModel.Navigation.Back();
			}
		}

		private void TryToSubscriveOnMenuEvents(IViewModel viewModel)
		{
		    // ReSharper disable once SuspiciousTypeConversion.Global
		    if (viewModel is IHaveSideMenu sideMenuViewModel)
			{
				sideMenuViewModel.MenuViewModel.PropertyChanged += (sender, e) =>
				{
					switch (e.PropertyName)
					{
						case "IsVisible":
							OnMenuVisibilityChanged(viewModel, sideMenuViewModel.MenuViewModel);
							break;
					}
				};
			}
		}

		private void OnMenuVisibilityChanged(IViewModel viewModel, ISideMenuViewModel menuViewModel)
		{
			try
			{
				if (menuViewModel.IsVisible)
				{
					menuViewModel.BecameVisible();
				}
				else
				{
					menuViewModel.BecameInvisible();
				}
			}
			catch (Exception e)
			{
				if (!viewModel.HandleException(e, () => OnMenuVisibilityChanged(viewModel, menuViewModel), () => OnCancel(viewModel)))
				{
					throw;
				}
			}
		}
	}
}

