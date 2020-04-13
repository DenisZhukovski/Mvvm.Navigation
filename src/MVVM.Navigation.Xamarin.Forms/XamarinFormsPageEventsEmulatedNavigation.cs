using System;
using System.Threading.Tasks;
using MVVMNavigation.Interfaces;
using MVVMNavigation.ViewModelLifeCycle;
using MVVMNavigation.ViewModels;

namespace MVVMNavigation.Navigation
{
	public class XamarinFormsPageEventsEmulatedNavigation : Navigation
	{
		protected readonly IHistoryStack HistoryStack;

		protected readonly IViewModelLifeCycleHandler ViewModelLifeCycleHandler;

		public XamarinFormsPageEventsEmulatedNavigation(
			IViewModelResolver viewModelResolver,
			IStack stack,
			IViewModelLifeCycleHandler viewModelLifeCycleHandler)
			: this(viewModelResolver, new HistoryStack.HistoryStack(stack), viewModelLifeCycleHandler)
		{
		}

		private XamarinFormsPageEventsEmulatedNavigation(
			IViewModelResolver viewModelResolver,
			IHistoryStack historyStack,
			IViewModelLifeCycleHandler viewModelLifeCycleHandler)
			: base(viewModelResolver, historyStack, viewModelLifeCycleHandler)
		{
			HistoryStack = historyStack;
			ViewModelLifeCycleHandler = viewModelLifeCycleHandler;
		}

		public override async Task<IViewModel> To(Type viewModelType, bool animated)
		{
			var viewModel = await base.To(viewModelType, animated);
			ViewModelLifeCycleHandler.OnAppearing(viewModel);
			return viewModel;
		}

		public override async Task<TViewModel> To<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject, bool animated)
		{
			var viewModel = await base.To<TViewModel, TViewModelNavigationObject>(navigationObject, animated);
			ViewModelLifeCycleHandler.OnAppearing(viewModel);
			return viewModel;
		}
	}
}
