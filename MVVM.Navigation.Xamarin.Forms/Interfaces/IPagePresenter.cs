using System;
using MVVMNavigation.Interfaces;
using Xamarin.Forms;

namespace MVVM.Navigation.Xamarin.Forms.Interfaces
{
	public interface IPagePresenter
    {
        Page Page<TViewModel>() where TViewModel: IViewModel;
        Page Page(Type viewModelType);
        Page Page<TViewModel>(TViewModel viewModel) where TViewModel: IViewModel;
        string PageName<TViewModel>() where TViewModel: IViewModel;
        string PageName(Type viewModelType);
	    NavigationPage NavigationPage(Page page);
    }
}

