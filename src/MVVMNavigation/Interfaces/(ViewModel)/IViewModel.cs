namespace MVVMNavigation.Interfaces
{
	public interface IViewModel : IViewModelLifeCycle
	{
		INavigation Navigation { get; }

		bool IsNavigatable { get; }
	}
}

