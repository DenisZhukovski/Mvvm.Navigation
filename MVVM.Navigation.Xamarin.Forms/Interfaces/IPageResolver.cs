using Xamarin.Forms;

namespace MVVM.Navigation.Xamarin.Forms.Interfaces
{
	public interface IPageResolver
	{
		Page Page(string pageName);
	}
}
