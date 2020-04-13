using System;
using System.Threading.Tasks;
using MVVMNavigation.Interfaces;

namespace MVVMNavigation.HistoryStack
{
	public class HistoryStack : IHistoryStack
	{
		private readonly IStack _stack;

	    public int Count => _stack.Count;

        public HistoryStack(IStack stack)
		{
			_stack = stack;
		}

		#region IHistoryStack members

		Task IHistoryStack.Clear()
		{
			return _stack.Clear();
		}

		public int IndexOf(Type viewModelType)
		{
			return _stack.IndexOf(viewModelType);
		}

		public int IndexOf<TViewModel>() where TViewModel : IViewModel
		{
			return _stack.IndexOf<TViewModel>();
		}

		bool IHistoryStack.IsAlreadyOnTopOfTheHistoryStack(Type type)
		{
			var currentViewModel = (this as IHistoryStack).Current();

			return currentViewModel != null && currentViewModel.GetType() == type;
		}

		bool IHistoryStack.WillBackTo(Type type)
		{
			var viewModelAfterPop = (this as IHistoryStack).TopAfterPop();

			return viewModelAfterPop != null && viewModelAfterPop.GetType() == type;
		}

		IViewModel IHistoryStack.Current()
		{
			return _stack.Current();
		}

		Task IHistoryStack.Push(IViewModel viewModel, bool animated)
		{
			return _stack.Push(viewModel, animated);
		}

		Task<IViewModel> IHistoryStack.Pop(bool animated)
		{
			return _stack.Pop(animated);
		}

		public Task<IViewModel> BackTo(Type viewModelType, bool animated)
		{
			return _stack.PopTo(viewModelType, animated);
		}

		public IViewModel FindInStack(Type viewModelType)
		{
			return _stack.FindInStack(viewModelType);
		}

		public IViewModel TopAfterPop()
		{
			return _stack.TopAfterPop();
		}

	    #endregion
	}
}

