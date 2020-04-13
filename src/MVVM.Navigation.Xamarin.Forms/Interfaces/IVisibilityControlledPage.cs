using System;

namespace MVVM.Navigation.Xamarin.Forms.Interfaces
{
	public interface IVisibilityControlledPage
	{
		bool IsVisibleOnTheScreen { get; }

		void ViewWillAppear();

		void ViewWillDisappear();

		void DidEnterBackground();

		event Action WillAppear;
	}
}