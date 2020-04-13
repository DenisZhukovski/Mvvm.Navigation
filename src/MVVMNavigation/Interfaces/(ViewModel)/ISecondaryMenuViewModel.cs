using System.Collections.Generic;
using System.Windows.Input;

namespace MVVMNavigation.Interfaces
{
	public interface ISecondaryMenuViewModel
	{
		IEnumerable<string> Items { get; }

		ICommand SelectCommand { get; set; }
	}
}
