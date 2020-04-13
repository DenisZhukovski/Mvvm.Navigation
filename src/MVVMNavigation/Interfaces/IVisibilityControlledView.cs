using System;

namespace MVVMNavigation.Interfaces
{
	public interface IVisibilityControlledView
	{
		bool IsVisibleOnTheScreen { get; }

		void ViewWillAppear();

		void ViewWillDisappear();

		void DidEnterBackground();

		event Action WillAppear;
	}
}