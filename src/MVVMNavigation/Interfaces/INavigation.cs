using System;
using System.Threading.Tasks;

namespace MVVMNavigation.Interfaces
{
	public interface INavigation : Dotnet.Commands.INavigation
    {
        Task<IViewModel> Back();
        Task<IViewModel> Back(bool animated, bool forceReloadData = false);
        Task<TViewModel> BackTo<TViewModel>() where TViewModel : IViewModel;
		Task<TViewModel> BackTo<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject) where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>;
		Task<TViewModel> BackTo<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject, bool animated, bool forceReloadData = false) where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>;
		Task<TViewModel> BackTo<TViewModel>(bool animated, bool forceReloadData = false) where TViewModel : IViewModel;
		Task<IViewModel> BackTo(Type viewModelType);
		Task<IViewModel> BackTo(Type viewModelType, bool animated, bool forceReloadData = false);

        bool WillBackTo<TViewModel>() where TViewModel : IViewModel;

        Task<IViewModel> To(Type viewModelType);
		Task<IViewModel> To(Type viewModelType, bool animated);
		Task<TViewModel> To<TViewModel>() where TViewModel : IViewModel, IViewModelWithoutParameter;
		Task<TViewModel> To<TViewModel>(bool animated) where TViewModel : IViewModel, IViewModelWithoutParameter;
		Task<TViewModel> To<TViewModel, TViewModelNavigationObject>(TViewModelNavigationObject navigationObject) where TViewModel : IViewModel, IViewModelWithParameter<TViewModelNavigationObject>;

        IViewModel Current();
        bool HasInHistory<TViewModel>() where TViewModel : IViewModel;
        Task Clear();

        event Action<Type> NavigationComplete;
    }
}
