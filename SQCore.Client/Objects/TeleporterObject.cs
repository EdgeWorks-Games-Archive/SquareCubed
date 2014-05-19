using System;
using OpenTK;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : IClientObject
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly UnitProximityHelper _proximity;

		private string _testDest = "Dest A";
		private const string TextPattern = "Destination: {0} (E to Change)";

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

		private void Update(object s, TickEventArgs e)
		{
			_proximity.Update(_client.Player);
		}

		public void OnUse()
		{
			if(_proximity.Status == ProximityStatus.Within)
				Console.WriteLine("Teleport me!");
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