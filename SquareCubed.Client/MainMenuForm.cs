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

			var playerLabel = new GuiLabel("Player Name")
			{
				Position = new Point(6, 3)
			};
			Controls.Add(playerLabel);

			var serverLabel = new GuiLabel("Server Address")
			{
				Position = new Point(6, playerLabel.Position.Y + playerLabel.Size.Height + 3)
			};
			Controls.Add(serverLabel);
		}
	}
}