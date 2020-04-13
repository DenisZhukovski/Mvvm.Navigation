using MVVMNavigation.Interfaces;
using Xamarin.Forms;

namespace MVVM.Navigation.Xamarin.Forms.Interfaces
{
	public interface IPageLifeCycleHandler
	{
		void OnCreated(Page page, Page prevPage, IViewModel viewModel);

		void WillAppear(Page page);

		void WillDisappear(Page page);

		void DidEnterBackground(Page page);
	}
}
