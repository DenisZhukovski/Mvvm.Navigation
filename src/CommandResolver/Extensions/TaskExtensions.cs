using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommandResolver.Extensions
{
	internal static class TaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void NoWarning(this Task task)
		{
			// Nothing to do here
		}
	}
}
