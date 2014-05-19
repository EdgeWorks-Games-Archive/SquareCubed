using System;
using Lidgren.Network;
using OpenTK;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Network;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : IClientObject
	{
		private const string TextPattern = "Destination: {0} (E to Change)";
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly UnitProximityHelper _proximity;

		private string _testDest = "Dest A";

		public TeleporterObject(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_client = client;
			_panel = panel;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new UnitProximityHelper(this, 0.5f);
			_proximity.Change += OnProximityChange;
		}

		public int Id { get; set; }
		public Vector2 Position { get; set; }

		public void OnUse()
		{
			// If not within use distance don't do anything
			if (_proximity.Status != ProximityStatus.Within) return;

			// Send teleport request to server
			var msg = _client.Structures.ObjectsNetwork.CreateMessageFor(this);
			_client.Network.SendToServer(msg, NetDeliveryMethod.ReliableUnordered, 0);
		}

		private void Update(object s, TickEventArgs e)
		{
			_proximity.Update(_client.Player);
		}

		private void OnKeyPress(object sender, KeyboardKeyEventArgs e)
		{
			// If not within or incorrect key, don't bother anything
			if (_proximity.Status != ProximityStatus.Within || e.Key != Key.E) return;

			_testDest = _testDest == "Dest A" ? "Dest B" : "Dest A";

			_panel.Text = string.Format(TextPattern, _testDest);
		}

		private void OnProximityChange(object s, ProximityEventArgs e)
		{
			if (e.NewStatus == ProximityStatus.Within)
			{
				_panel.Text = string.Format(TextPattern, _testDest);
				_panel.IsVisible = true;
			}
			else
				_panel.IsVisible = false;
		}
	}
}