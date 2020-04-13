namespace MVVMNavigation.Interfaces
{
	public interface IViewModelWithParameter<in TNavigationParameter>
	{
		void ViewModelInit(TNavigationParameter navigationParameter);
	}

	public interface IViewModelWithoutParameter
	{
	}
}
