using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Gui.Panels
{
	public sealed class MainMenu : IDisposable
	{
		private MainMenuPanel _menuPanel;

		public void Open(SquareCubed.Client.Gui.Gui gui, Network.Network network)
		{
			Contract.Requires<ArgumentNullException>(gui != null);
			Contract.Requires<ArgumentNullException>(network != null);

			_menuPanel = new MainMenuPanel(gui);
			network.NewConnection += (s, e) => Close();
		}

		public void Close()
		{
			_menuPanel.Dispose();
			_menuPanel = null;
		}

		public void Dispose()
		{
			Close();
		}
	}
}
