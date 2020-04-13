using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommandResolver.Interfaces
{
	public interface ICommandResolver
	{
		bool IsLocked { get; }

		ICommand AsyncCommand(Func<Task> execute, Func<bool> canExecute = null);

        ICommand AsyncCommand<TParam>(Func<TParam, Task> execute, Func<TParam, bool> canExecute = null);

		ICommand Command(Action execute, Func<bool> canExecute = null);

		ICommand Command<TParam>(Action<TParam> execute, Func<TParam, bool> canExecute = null);

		void ForceRelease();
	}
}
