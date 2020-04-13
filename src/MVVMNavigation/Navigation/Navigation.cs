using System;
using System.Threading.Tasks;
using MVVMNavigation.Extensions;
using MVVMNavigation.Interfaces;

namespace MVVMNavigation.Navigation
{
	public class Navigation : INavigation
	{
		private readonly IHistoryStack _historyStack;
		private readonly IViewModelLifeCycleHandler _viewModelLifeCycle;
		private readonly IViewModelResolver _viewModelResolver;

        public event Action<Type> NavigationComplete;

        public bool IsNavigationInProgress { get; private set; }

		public Navigation(
            IViewModelResolver viewModelResolver, 
            IHistoryStack historyStack, 
            IViewModelLifeCycleHandler viewModelLifeCycle)
		{
			_viewModelResolver = viewModelResolver;
			_historyStack = historyStack;
			_viewModelLifeCycle = viewModelLifeCycle;
		}

		public async Task Clear()
		{
			await _historyStack.Clear();
			IsNavigationInProgress = false;
		}

        public bool HasInHistory<TViewModel>() where TViewModel : IViewModel
		{
			return _historyStack.IndexOf(typeof(TViewModel)) > -1;
		}

		public void NavigationCompleted(Type viewModelType)
		{
			IsNavigationInProgress = false;
            if (NavigationComplete != null)
            {
                NavigationComplete(viewModelType);
            }
		}

		public Task<IViewModel> Back()
		{
			return Back(true);
		}

		public Task<TViewModel> BackTo<TViewModel>() where TViewModel : IViewModel
		{
			return BackTo<TViewModel>(true);
		}

		public bool WillBackTo<TViewModel>() where TViewModel : IViewModel
		{
			return _historyStack.WillBackTo(typeof(TViewModel));
		}

		public virtual async Task<IViewModel> Back(bool animated, bool forceReloadData = false)
		{
			var viewModel = _historyStack.TopAfterPop();
			if (viewModel != null)
			{
				_viewModelLifeCycle.ViewModelWillAppear(viewModel, viewModel.ReloadOnEachShowing || forceReloadData).InBackground();
			}

			DisappearCurrentViewModel();
			await _historyStack.Pop(animated);
			return _historyStack.Current();
		}

		public virtual async Task<TViewModel> BackTo<TViewModel>(bool animated, bool forceReload = false) where TViewModel : IViewModel
		{
			var viewModel = await BackTo(typeof(TViewModel), animated, forceReload);
			return (TViewModel)viewModel;
		}

		public Task<TViewModel> BackTo<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject)
			where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>
		{
			return BackTo<TViewModel, TViewModelNavigationObject>(navigationObject, true);
		}

		public async Task<TViewModel> BackTo<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject, bool animated, bool forceReload = false)
			where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>
		{
			var viewModel = (TViewModel)_historyStack.FindInStack(typeof(TViewModel));
			if (viewModel != null)
			{
				_viewModelLifeCycle.InitWithParameter(viewModel, navigationObject);
			}

			await BackTo<TViewModel>(animated, forceReload);
			return viewModel;
		}

		public Task<IViewModel> BackTo(Type viewModelType)
		{
			return BackTo(viewModelType, true);
		}

		public async Task<IViewModel> BackTo(Type viewModelType, bool animated, bool forceReloadData = false)
		{
			var viewModel = _historyStack.FindInStack(viewModelType);
			if (viewModel != null)
			{
				_viewModelLifeCycle.ViewModelWillAppear(viewModel, viewModel.ReloadOnEachShowing || forceReloadData).InBackground();
			}

			DisappearCurrentViewModel();
			return await _historyStack.BackTo(viewModelType, animated);
		}

		public IViewModel Current()
		{
			return _historyStack.Current();
		}

		public virtual Task<TViewModel> To<TViewModel>() where TViewModel : IViewModel, IViewModelWithoutParameter
		{
			return To<TViewModel>(true);
		}

		public virtual async Task<TViewModel> To<TViewModel>(bool animated) where TViewModel : IViewModel, IViewModelWithoutParameter
		{
			var viewModel = await To(typeof(TViewModel), animated);
			return (TViewModel)viewModel;
		}

		public virtual Task<IViewModel> To(Type viewModelType)
		{
			return To(viewModelType, true);
		}

		public virtual async Task<IViewModel> To(Type viewModelType, bool animated)
		{
			if (!NavigationBegan(viewModelType))
			{
				return null;
			}

			IViewModel viewModel;
			try
			{
				DisappearCurrentViewModel();
				viewModel = ViewModel(viewModelType);

				await To(viewModel, animated).ConfigureAwait(false);
			}
			catch (Exception)
			{
				IsNavigationInProgress = false;
				throw;
			}

			return viewModel;
		}

		public virtual Task<TViewModel> To<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject)
			where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>
		{
			return To<TViewModel, TViewModelNavigationObject>(navigationObject, true);
		}

		public virtual async Task<TViewModel> To<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject, bool animated)
			where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>
		{
			if (!NavigationBegan(typeof(TViewModel)))
			{
				return default(TViewModel);
			}

			TViewModel viewModel;
			try
			{
				DisappearCurrentViewModel();

				viewModel = ViewModel<TViewModel>();
				_viewModelLifeCycle.InitWithParameter(viewModel, navigationObject);

				await To(viewModel, animated).ConfigureAwait(false);
			}
			catch (Exception)
			{
				IsNavigationInProgress = false;
				throw;
			}

			return viewModel;
		}

		protected virtual TViewModel ViewModel<TViewModel>() where TViewModel : IViewModel
		{
			return _viewModelResolver.ViewModel<TViewModel>();
		}

		protected virtual IViewModel ViewModel(Type viewModelType)
		{
			return _viewModelResolver.ViewModel(viewModelType);
		}

		private Task<IViewModel> To(IViewModel viewModel, bool animated)
		{
			_viewModelLifeCycle.OnCreated(viewModel);
			_viewModelLifeCycle.ViewModelWillAppear(viewModel, true).InBackground();

			_historyStack.Push(viewModel, animated).InBackground();
			return Task.FromResult(viewModel);
		}

		private bool NavigationBegan(Type viewModelType)
		{
			bool isOtherViewModel = !_historyStack.IsAlreadyOnTopOfTheHistoryStack(viewModelType);
			if (isOtherViewModel)
			{
				IsNavigationInProgress = true;
			}

			return isOtherViewModel;
		}

		private void DisappearCurrentViewModel()
		{
			var currentViewModel = _historyStack.Current();
			if (currentViewModel != null)
			{
				_viewModelLifeCycle.ViewModelWillDisappear(currentViewModel);
			}
		}
	}
}

