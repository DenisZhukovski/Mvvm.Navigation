using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVVMNavigation.Extensions
{
	internal static class TaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async void InBackground(this Task task, Action<Exception> onException = null)
		{
			// Nothing to do here
			try
			{
				await task;
			}
			catch (Exception e)
			{
				if (onException != null)
				{
					onException(e);
				}
			}
		}
	}
}
