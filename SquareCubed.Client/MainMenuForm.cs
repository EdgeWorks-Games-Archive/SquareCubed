using System.Drawing;
using SquareCubed.Client.Gui.Controls;

namespace SquareCubed.Client
{
	internal sealed class MainMenuForm : GuiForm
	{
		public MainMenuForm()
		{
			Title = "Connect to Server";
			InnerSize = new Size(300, 200);
		}
	}
}