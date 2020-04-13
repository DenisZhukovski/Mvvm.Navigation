using System;
using System.Threading.Tasks;

namespace MVVMNavigation.Interfaces
{
	public interface IViewModelLifeCycle
    {
        Task LoadData();

		bool IsDataLoaded { get; set; }

		bool IsLoading { get; set; }

		bool IsVisible { get; set; }

		bool ReloadOnEachShowing { get; }

		Task WillAppear(bool needLoadData);

	    void WillDisappear();

		void BecameVisible();

	    void BecameInvisible();

	    bool HandleException(Exception e, Action retry, Action cancel);
    }
}

