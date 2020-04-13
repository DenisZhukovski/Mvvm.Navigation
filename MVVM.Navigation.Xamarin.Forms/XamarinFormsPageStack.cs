using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVM.Navigation.Xamarin.Forms.Interfaces;
using MVVM.Navigation.Xamarin.Forms.Extensions;
using MVVMNavigation.Interfaces;
using Xamarin.Forms;
using MasterDetailPage = Xamarin.Forms.MasterDetailPage;

namespace MVVMNavigation.HistoryStack
{
    public class XamarinFormsPageStack : IStack
	{
		private readonly Application _application;
		private readonly IPagePresenter _pagePresenter;
		private readonly IPageLifeCycleHandler _formsPageLifeCycleHandler;
        private bool _cleared;

		public XamarinFormsPageStack(
            Application application, 
            IPagePresenter pagePresenter, 
            IPageLifeCycleHandler formsPageLifeCycleHandler)
		{
			_application = application;
			_pagePresenter = pagePresenter;
			_formsPageLifeCycleHandler = formsPageLifeCycleHandler;
		}

		public Page TopPage()
		{
			return TopPage(true);
		}

		public Page TopPage(bool needContentPage)
		{
			Page page = null;
			if (Exists())
			{
				IReadOnlyList<Page> list = _application.MainPage.Navigation.NavigationStack;

                // Here list.Last() element is on top of the stack 
                page = list.Any() ? list.Last() : _application.MainPage;
				page = needContentPage ? page.ContentPage() : page;
			}

			return page;
		}

		public Page TopPageAfterPop()
		{
			if (_application.MainPage.Navigation.NavigationStack.Count > 1)
			{
				Page page = null;
				var index = _application.MainPage.Navigation.NavigationStack.Count - 2;

				// this loop is necessary because during back operation some other back operation can be called
				while (index >= 0 && page == null)
				{
					page  = _application.MainPage.Navigation.NavigationStack[index];
					var visiblePage = page as IVisibilityControlledPage;
					if (visiblePage != null && visiblePage.IsVisibleOnTheScreen)
					{
						index--;
						page = null;
					}
				}

				return page;

			}

			return null;
		}

		public Task Clear()
		{
            _cleared = true;

            if (Exists() && _application.MainPage.Navigation.NavigationStack.Any())
			{
                return _application.MainPage.Navigation.PopToRootAsync(false);
            }

            return Task.CompletedTask;
		}

        public int IndexOf(Type viewModelType)
		{
			return IndexOf(_pagePresenter.PageName(viewModelType));
		}

		public int IndexOf<TViewModel>() where TViewModel : IViewModel
		{
			return IndexOf(_pagePresenter.PageName<TViewModel>());
		}

		public IViewModel FindInStack(Type viewModelType)
		{
			return new HistoryStackPage(_application.MainPage).ViewModel(_pagePresenter.PageName(viewModelType));
		}

		public IViewModel Current()
		{
			return Exists() ? TopPage().BindingContext as IViewModel : null;
		}

		public Task Push(IViewModel viewModel, bool animated)
		{
			return Push(_pagePresenter.Page(viewModel), viewModel, animated);
		}

		async Task<IViewModel> IStack.Pop(bool animated)
		{
			PageWillDisappear(TopPage());
			var topPage = TopPageAfterPop();
			if (topPage != null)
			{
				_formsPageLifeCycleHandler.WillAppear(topPage.ContentPage());
			}

			var page = await PopPage(animated);
			return page != null ? ViewModel(page) : null;
		}

		public IViewModel TopAfterPop()
		{
			IViewModel viewModel = null;
			var topPage = TopPageAfterPop();
			if (topPage != null)
			{
				viewModel = ViewModel(topPage.ContentPage());
			}

			return viewModel;
		}

		public async Task<IViewModel> PopTo(Type viewModelType, bool animated)
		{
			IViewModel viewModel = null;
			var viewModelIndex = IndexOf(viewModelType);
			if (viewModelIndex > -1)
			{
				var counter = 1;
				var source = new TaskCompletionSource<IViewModel>();
				PageWillDisappear(TopPage());
				Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        while (Current().GetType() != viewModelType)
                        {
                            if (counter == viewModelIndex)
                            {
                                _formsPageLifeCycleHandler.WillAppear(TopPageAfterPop().ContentPage());
                                await _application.MainPage.Navigation.PopAsync(animated);
                            }
                            else
                            {
                                _application.MainPage.Navigation.RemovePage(TopPageAfterPop());
                            }

                            counter++;
                        }

                        source.SetResult(Current());
                    }
                    catch(Exception e)
                    {
                        source.SetException(e);
                    }
				});

                viewModel = await source.Task;
			}

			return viewModel;
		}

		private bool Exists()
		{
			return _application.HasMainPage();
		}

		private int IndexOf(string pageName)
		{
			return new HistoryStackPage(_application.MainPage).Index(pageName);
		}

		private Task Push(Page page, IViewModel viewModel, bool animated)
		{
			if (!IsAlreadyOnTopOfTheHistoryStack(page))
			{
				var topPage = TopPage();
				PageWillDisappear(topPage);
				_formsPageLifeCycleHandler.OnCreated(page, topPage, viewModel);
				var nextPage = NextPage(page);
				if (!_application.HasMainPage() || _cleared)
				{
                    _cleared = false;
                    _formsPageLifeCycleHandler.WillAppear(page.ContentPage());
					_application.MainPage = nextPage;
				}
				else
				{
					var navigationPage = _application.MainPage.NavigationPage();
					RaiseWillAppearEventAfterAddedToNavigationPage(navigationPage);
					return navigationPage.Navigation.PushAsync(nextPage, animated);
				}
			}

			return Task.FromResult(true);
		}

		private void PageWillDisappear(Page page)
		{
			if (page != null)
			{
				_formsPageLifeCycleHandler.WillDisappear(page.ContentPage());
			}
		}

		/// <summary>
		/// this methid is necessary because otherwise page in willAppear method will be without Parent
		/// </summary>
		/// <param name="navigationPage"></param>
		private void RaiseWillAppearEventAfterAddedToNavigationPage(Page navigationPage)
		{
			EventHandler<ElementEventArgs> childAdded = null;
			childAdded = (sender, args) =>
			{
				navigationPage.ChildAdded -= childAdded;
				var navigatedPage = args.Element as Page;
				if (navigatedPage != null)
				{
					_formsPageLifeCycleHandler.WillAppear(navigatedPage.ContentPage());
				}
			};

			navigationPage.ChildAdded += childAdded;
		}

		private Task<Page> PopPage(bool animated)
		{
			if (Exists() && _application.MainPage.Navigation.NavigationStack.Any())
			{
				return _application.MainPage.Navigation.PopAsync(animated);
			}

			return Task.FromResult((Page)null);
		}

		private Page NextPage(Page page)
		{
            if (!Exists() || _cleared)
			{
				if (IsNotNavigatable(page))
				{
					return page;
				}

				return _pagePresenter.NavigationPage(page);
			}

			return page;
		}

		private bool IsNotNavigatable(Page page)
		{
			return page is NavigationPage || page is MasterDetailPage;
		}

		private bool IsAlreadyOnTopOfTheHistoryStack(Page page)
		{
			return TopPage() == page.ContentPage();
		}

		private IViewModel ViewModel(Page page)
		{
			return page.BindingContext as IViewModel;
		}

		public int Count
		{
			get
			{
				if (!Exists())
				{
					return 0;
				}

				return _application.MainPage.Navigation.NavigationStack.Count;
			}
		}
	}
}
