using System;

namespace CommandResolver.Interfaces
{
	public interface INavigation
	{
		bool IsNavigationInProgress { get; }

		void NavigationCompleted(Type viewModelType);
	}
}
