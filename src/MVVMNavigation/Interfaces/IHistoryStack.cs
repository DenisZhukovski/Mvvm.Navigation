﻿using System;
using System.Threading.Tasks;

namespace MVVMNavigation.Interfaces
{
	public interface IHistoryStack
	{
        int Count { get; }

        IViewModel Current();

		Task<IViewModel> Pop(bool animated);

		IViewModel TopAfterPop();

		int IndexOf<TViewModel>() where TViewModel : IViewModel;

		int IndexOf(Type viewModelType);

		IViewModel FindInStack(Type viewModelType);

		Task Push(IViewModel viewModel, bool animated);

		Task Clear();

        bool IsAlreadyOnTopOfTheHistoryStack(Type viewModelType);

		Task<IViewModel> BackTo(Type viewModelType, bool animated);

		bool WillBackTo(Type viewModelType);
    }
}
