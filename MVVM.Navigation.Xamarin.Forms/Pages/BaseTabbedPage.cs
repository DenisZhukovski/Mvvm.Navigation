using System;
using MVVM.Navigation.Xamarin.Forms.Interfaces;
using MVVMNavigation.Interfaces;
using MVVMNavigation.ViewModelLifeCycle;
using Xamarin.Forms;

namespace MVVMNavigation.Pages
{
	public class BaseTabbedPage : TabbedPage, IVisibilityControlledPage
	{
		public bool IsVisibleOnTheScreen { get; private set; }

		public event Action WillAppear = delegate { };

		protected IViewModelLifeCycleHandler ViewModelLifeCycleHandler { get; set; }

		public BaseTabbedPage(IViewModelLifeCycleHandler viewModelLifeCycleHandler)
		{
			ViewModelLifeCycleHandler = viewModelLifeCycleHandler;
		}

		void IVisibilityControlledPage.ViewWillAppear()
		{
			IsVisibleOnTheScreen = true;
			OnWillAppear();
			WillAppear();
		}

		void IVisibilityControlledPage.ViewWillDisappear()
		{
			OnWillDisappear();
		}

		protected virtual void OnWillAppear()
		{
		}

		protected virtual void OnWillDisappear()
		{
			
		}

		void IVisibilityControlledPage.DidEnterBackground()
		{
			if (IsVisibleOnTheScreen)
			{
				ViewModelLifeCycleHandler.DidEnterBackground(BindingContext as IViewModel);
			}

			IsVisibleOnTheScreen = false;
			OnDidEnterBackground();
		}

		protected virtual void OnDidEnterBackground()
		{
		}

		protected override bool OnBackButtonPressed()
		{
			return ViewModelLifeCycleHandler.OnBackButtonPressed(BindingContext as IViewModel) || base.OnBackButtonPressed();
		}

		protected override void OnAppearing()
		{
			IsVisibleOnTheScreen = true;
			ViewModelLifeCycleHandler.OnAppearing(BindingContext as IViewModel);
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			IsVisibleOnTheScreen = false;
			ViewModelLifeCycleHandler.OnDisappearing(BindingContext as IViewModel);
		}
	}
}