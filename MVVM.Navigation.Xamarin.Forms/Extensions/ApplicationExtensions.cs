using Xamarin.Forms;

namespace MVVM.Navigation.Xamarin.Forms.Extensions
{
	public static class ApplicationExtensions
	{
		public static bool HasMainPage(this Application application)
		{
			return application.MainPage != null;
		}
	}
}
