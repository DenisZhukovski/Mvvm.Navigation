using System.Threading.Tasks;

namespace MVVMNavigation.Interfaces
{
	public interface IViewModelLifeCycleHandler
	{
		void InitWithParameter<TViewModelNavigationObject>(
			IViewModelWithParameter<TViewModelNavigationObject> viewModel,
			TViewModelNavigationObject navigationObject);

		void OnCreated(IViewModel viewModel);
		Task ViewModelWillAppear(IViewModel viewModel, bool needLoadData);
		void ViewModelWillDisappear(IViewModel currentViewModel);

		void DidEnterBackground(IViewModel viewModel);
		bool OnBackButtonPressed(IViewModel viewModel);
		void OnAppearing(IViewModel viewModel);
		void OnDisappearing(IViewModel viewModel);
	}
}
