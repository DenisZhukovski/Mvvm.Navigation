using System;
using System.Threading.Tasks;

namespace MVVMNavigation.Interfaces
{
	public interface IStack
	{
		Task Clear();

		int IndexOf(Type viewModelType);

		int IndexOf<TViewModel>() where TViewModel : IViewModel;

		IViewModel Current();

		Task Push(IViewModel viewModel, bool animated);

		Task<IViewModel> Pop(bool animated);

		Task<IViewModel> PopTo(Type viewModelType, bool animated);

		IViewModel FindInStack(Type viewModelType);

		IViewModel TopAfterPop();
		int Count { get; }
	}
}
