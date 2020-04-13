using System.Threading.Tasks;
using CommandResolver.Interfaces;

namespace CommandResolver
{
	public class CommandExecutionLock : ICommandExecutionLock
	{
		/// <summary>
		/// This interval is necessary to avoid multi tapping command from the user
		/// It can happen when user clicks simuntainiusly on several buttons on the screen
		/// </summary>
		public static int CommandExecutionInterval = 300;
		private readonly INavigation _navigation;
		private readonly object _lockObject;
		private bool _isExecutionLock;

		public CommandExecutionLock(INavigation navigation)
		{
			_navigation = navigation;
			_lockObject = new object();
			_isExecutionLock = false;
		}

		public bool IsLocked
		{
			get
			{
				lock (_lockObject)
				{
					return IsLockedImplementationNonBlocked;
				}
			}
		}

		public bool TryLockExecution()
		{
			if (IsLockedImplementationNonBlocked)
			{
				return false;
			}

			lock (_lockObject)
			{
				if (IsLockedImplementationNonBlocked)
				{
					return false;
				}

				return _isExecutionLock = true;
			}
		}

		public async Task<bool> FreeExecutionLock()
		{
			await Task.Delay(CommandExecutionInterval);
			if (!_isExecutionLock)
			{
				return false;
			}

			lock (_lockObject)
			{
				if (!_isExecutionLock)
				{
					return false;
				}


				_isExecutionLock = false;
				return true;
			}
		}

		private bool IsLockedImplementationNonBlocked
		{
			get { return _isExecutionLock || _navigation.IsNavigationInProgress; }
		}
	}
}