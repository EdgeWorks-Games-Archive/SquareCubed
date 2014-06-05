using System;
using System.Drawing;
using SquareCubed.Client.Gui.Controls;

namespace SquareCubed.Client
{
	internal sealed class MainMenuForm : GuiForm
	{
		private readonly GuiTextBox _playerName;
		private readonly GuiTextBox _serverAddress;

		public MainMenuForm()
			: base("Connect to Server")
		{
			InnerSize = new Size(250, 200);

			var playerLabel = new GuiLabel("Player Name")
			{
				Position = new Point(6, 3)
			};
			Controls.Add(playerLabel);

			_playerName = new GuiTextBox("Player")
			{
				Padding = 6,
				Width = InnerSize.Width,
				Position = new Point(0, playerLabel.Position.Y + playerLabel.Size.Height + 3)
			};
			Controls.Add(_playerName);

			var serverLabel = new GuiLabel("Server Address")
			{
				Position = new Point(6, _playerName.Position.Y + _playerName.Size.Height + 6)
			};
			Controls.Add(serverLabel);

			_serverAddress = new GuiTextBox("127.0.0.1")
			{
				Padding = 6,
				Width = InnerSize.Width,
				Position = new Point(0, serverLabel.Position.Y + serverLabel.Size.Height + 3)
			};
			Controls.Add(_serverAddress);

			var connectButton = new GuiButton("Connect")
			{
				Position = new Point(6, _serverAddress.Position.Y + _serverAddress.Size.Height + 12),
				Size = new Size(80, 21)
			};
			connectButton.Click += (s, e) => Connect.Invoke(this, new ConnectEventArgs(_playerName.Text, _serverAddress.Text));
			Controls.Add(connectButton);

			var quitButton = new GuiButton("Quit")
			{
				Position = new Point(InnerSize.Width - 80 - 6, _serverAddress.Position.Y + _serverAddress.Size.Height + 12),
				Size = new Size(80, 21)
			};
			quitButton.Click += (s, e) => Quit.Invoke(this, EventArgs.Empty);
			Controls.Add(quitButton);

			InnerSize = new Size(InnerSize.Width, quitButton.Position.Y + quitButton.Size.Height + 6);
		}

		public event EventHandler<ConnectEventArgs> Connect = (s, e) => { };
		public event EventHandler Quit = (s, e) => { };

		public class ConnectEventArgs : EventArgs
		{
			public ConnectEventArgs(string playerName, string hostName)
			{
				PlayerName = playerName;
				HostAddress = hostName;
			}

			public string PlayerName { get; private set; }
			public string HostAddress { get; private set; }
		}
	}
}