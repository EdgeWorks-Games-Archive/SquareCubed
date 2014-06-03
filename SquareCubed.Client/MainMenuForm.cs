using System.Drawing;
using SquareCubed.Client.Gui.Controls;

namespace SquareCubed.Client
{
	internal sealed class MainMenuForm : GuiForm
	{
		public MainMenuForm()
			: base("Connect to Server")
		{
			InnerSize = new Size(300, 200);
			Controls.Add(new GuiLabel("Label 1"));
		}
	}
}