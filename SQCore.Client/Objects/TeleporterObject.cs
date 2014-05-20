using Lidgren.Network;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : ClientObjectBase
	{
		private const string TextPattern = "Destination: {0} (E to Change)";
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly ProximityHelper _proximity;

		private string _testDest = "Dest A";

		public TeleporterObject(SquareCubed.Client.Client client, ContextInfoPanel panel, ClientStructure parent)
			: base(parent)
		{
			_client = client;
			_panel = panel;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new ProximityHelper(this, 0.5f);
			_proximity.Change += OnProximityChange;
		}

		public override void OnUse()
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
				_panel.VisibleCount++;
			}
			else
				_panel.VisibleCount--;
		}
	}
}