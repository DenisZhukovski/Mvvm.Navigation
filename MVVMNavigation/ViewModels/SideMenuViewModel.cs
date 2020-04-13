using CommandResolver.Interfaces;
using MVVMNavigation.Interfaces;
using INavigation = MVVMNavigation.Interfaces.INavigation;

namespace MVVMNavigation.ViewModels
{
	public abstract class SideMenuViewModel : ViewModel, ISideMenuViewModel
	{
		private bool _isSideMenuVisible;

		public override bool IsVisible
		{
			get => _isSideMenuVisible;
		    set 
			{
                SetProperty(ref _isSideMenuVisible, value);
			}
		}

		public string Icon { get; set; }

		protected SideMenuViewModel(INavigation navigation, ICommandResolver commandResolver) 
			: base(commandResolver, navigation)
		{
		}
	}
}
