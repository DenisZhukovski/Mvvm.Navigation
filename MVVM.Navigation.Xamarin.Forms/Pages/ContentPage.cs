using System;
using MVVM.Navigation.Xamarin.Forms.Interfaces;
using MVVMNavigation.Interfaces;

namespace MVVMNavigation.Pages
{
    public class ContentPage : Xamarin.Forms.ContentPage, IVisibilityControlledPage
	{
		public bool IsVisibleOnTheScreen { get; private set; }

		public event Action WillAppear = delegate { };

		protected IViewModelLifeCycleHandler ViewModelLifeCycleHandler { get; set; }

		#region IVisibilityControlledPage members

		void IVisibilityControlledPage.ViewWillAppear()
		{
			IsVisibleOnTheScreen = true;
			OnWillAppear();
			WillAppear();
		}

		void IVisibilityControlledPage.ViewWillDisappear()
		{
            IsVisibleOnTheScreen = false;
			OnWillDisappear();
		}

		void IVisibilityControlledPage.DidEnterBackground()
		{
			IsVisibleOnTheScreen = false;
			OnDidEnterBackground();
		}

		#endregion

		public ContentPage(IViewModelLifeCycleHandler viewModelLifeCycleHandler)
		{
			ViewModelLifeCycleHandler = viewModelLifeCycleHandler;
		}

		protected virtual void OnWillAppear()
		{
		}

		protected virtual void OnWillDisappear()
		{
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
