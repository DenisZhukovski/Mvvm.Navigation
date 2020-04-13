using System.Threading.Tasks;
using System.Windows.Input;

namespace CommandResolver.Interfaces
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync(object parameter);

	    void RaiseCanExecuteChanged();
	}

	public interface IAsyncCommand<in TParam> : ICommand
    {
		Task ExecuteAsync(TParam parameter);

        void RaiseCanExecuteChanged();
    }
}
