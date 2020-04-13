using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMNavigation.Interfaces;

namespace MVVMNavigation.HistoryStack
{
	public class FakeHistoryStack : IStack
	{
		private readonly Stack<IViewModel> _viewModelsStack = new Stack<IViewModel>();

	    public int Count => _viewModelsStack.Count;

        public IViewModel Current()
		{
			return _viewModelsStack.Any() ? _viewModelsStack.Peek() : null;
		}

		public IViewModel TopAfterPop()
		{
			IViewModel viewModel = null;
			if (_viewModelsStack.Count > 1)
			{
				var index = 1;
				while (index < _viewModelsStack.Count && viewModel == null)
				{
					viewModel = _viewModelsStack.ElementAtOrDefault(index);
					if (viewModel != null && viewModel.IsVisible)
					{
						viewModel = null;
						index++;
					}
				}
			}

			return viewModel;
		}

		public Task<IViewModel> Pop(bool animated)
		{
			return Task.FromResult(_viewModelsStack.Pop());
		}

		public Task<TViewModel> PopTill<TViewModel>(bool animated) where TViewModel : IViewModel
		{
			TViewModel result = default(TViewModel);
			if (IndexOf<TViewModel>() > -1)
			{
				do
				{
					var viewModelFromStack = _viewModelsStack.First();
					if (viewModelFromStack is TViewModel)
					{
						result = (TViewModel)viewModelFromStack;
					}
					else
					{
						_viewModelsStack.Pop();
					}
				} while (result == null && _viewModelsStack.Any());
			}

			return Task.FromResult(result);
		}

		public int IndexOf<TViewModel>() where TViewModel : IViewModel
		{
			return IndexOf(typeof(TViewModel));
		}

		public int IndexOf(Type viewModelType)
		{
			int index = -1;
			int counter = 0;
			foreach (var viewModel in _viewModelsStack)
			{
				if (viewModel.GetType() == viewModelType)
				{
					index = counter;
					break;
				}

				counter++;
			}

			return index;
		}

		public virtual Task Push(IViewModel viewModel, bool animated)
		{
			_viewModelsStack.Push(viewModel);
			return Task.FromResult(true);
		}

		public Task Clear()
		{
			_viewModelsStack.Clear();
			return Task.FromResult(true);
		}

		public Task<IViewModel> PopTo(Type viewModelType, bool animated)
		{
			IViewModel viewModel = null;
			if (IndexOf(viewModelType) > -1)
			{
				while (Current().GetType() != viewModelType)
				{
					_viewModelsStack.Pop();
				}

				viewModel = Current();
			}

			return Task.FromResult(viewModel);
		}

		public IViewModel FindInStack(Type viewModelType)
		{
			IViewModel foundViewModel = null;
			foreach (var viewModel in _viewModelsStack)
			{
				if (viewModel.GetType() == viewModelType)
				{
					foundViewModel = viewModel;
					break;
				}
			}

			return foundViewModel;
		}
	}
}
