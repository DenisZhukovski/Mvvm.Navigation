using System;
using System.Threading.Tasks;
using Dotnet.Commands;
using MVVM.Navigation;
using MVVMNavigation.Interfaces;
using INavigation = MVVMNavigation.Interfaces.INavigation;

namespace MVVMNavigation.ViewModels
{
	public abstract class ViewModel : BindableObject, IViewModel, IHaveBackButton
	{
		protected INavigation Navigation { get; }

		protected readonly ICommands Commands;

		#region IHaveBackButton members

		bool IHaveBackButton.Back()
		{
			return OnHandleBack();
		}

		#endregion

		#region IViewModel members

		INavigation IViewModel.Navigation => Navigation;

	    #endregion

		#region IViewModelLifeCycle members

		public virtual bool IsDataLoaded { get; set; }

		public bool IsLoading { get; set; }

		public virtual bool IsVisible { get; set; }

		public virtual bool ReloadOnEachShowing => false;

	    public Task WillAppear(bool dataWillBeLoaded)
		{
			IsVisible = true;
			return OnWillAppear(dataWillBeLoaded);
		}

		public void WillDisappear()
		{
			IsVisible = false;
		}

		public void BecameVisible()
		{
			IsVisible = true;
			OnBecameVisible();
		}

		public void BecameInvisible()
		{
			IsVisible = false;
			OnBecameInvisible();
		}

		public virtual bool IsNavigatable => true;

	    public abstract bool HandleException(Exception e, Action retry, Action cancel);

        #endregion

        protected ViewModel(ICommands commands, INavigation navigation)
		{
            Commands = commands.Cached();
			Navigation = navigation;
		}

		public virtual Task LoadData()
		{
			return Task.FromResult(true);
		}

		protected virtual Task OnWillAppear(bool dataWillBeLoaded)
		{
			return Task.FromResult(true);
		}

		protected virtual void OnBecameVisible()
		{
		}

		protected virtual void OnBecameInvisible()
		{
		}

		protected virtual bool OnHandleBack()
		{
			return false;
		}
	}
}

