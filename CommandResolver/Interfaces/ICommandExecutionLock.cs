using System.Threading.Tasks;

namespace CommandResolver.Interfaces
{
	public interface ICommandExecutionLock
	{
		bool TryLockExecution();

		Task<bool> FreeExecutionLock();

		bool IsLocked { get; }
	}
}