using System;

namespace MVVMNavigation.Interfaces
{
	public interface IViewModelResolver
    {
        TViewModel ViewModel<TViewModel>() where TViewModel: IViewModel;

        IViewModel ViewModel(Type viewModelType);
    }
}

