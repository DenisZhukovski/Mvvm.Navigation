using System.Windows.Input;

namespace MVVMNavigation.ViewModels
{
	public class MenuItem
    {
        public MenuItem(string icon, string text, ICommand selectItemCommand)
        {
            Icon = icon;
            Text = text;
            SelectItemCommand = selectItemCommand;
        }

        public string Icon { get; set; }

        public string Text { get; set; }

		public ICommand SelectItemCommand { get; set; }
	}
}

