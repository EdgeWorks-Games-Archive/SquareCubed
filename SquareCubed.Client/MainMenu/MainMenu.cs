using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.MainMenu
{
	public class MainMenu
	{
		private MainMenuPanel _menuPanel;

		public void Create(Gui.Gui gui, Network.Network network)
		{
			Contract.Requires<ArgumentNullException>(gui != null);
			Contract.Requires<ArgumentNullException>(network != null);

			_menuPanel = new MainMenuPanel(gui);
			network.NewConnection += (s, e) => Remove();
		}

		public void Remove()
		{
			_menuPanel.Dispose();
			_menuPanel = null;
		}
	}
}
