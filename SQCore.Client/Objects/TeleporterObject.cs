using Lidgren.Network;
using OpenTK;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : ClientObjectBase
	{
		private const string Text = "Press E to Teleport";
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly ProximityHelper _proximity;
		private readonly TeleporterObjectType _type;

		public TeleporterObject(ClientStructure parent, TeleporterObjectType type, SquareCubed.Client.Client client,
			ContextInfoPanel panel)
			: base(parent)
		{
			_type = type;
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

			// Open the dialog for the teleporter
			_type.OpenDialog();
		}

		private void Update(object s, TickEventArgs e)
		{
			_proximity.Update(_client.Player);
		}

		private void OnKeyPress(object sender, KeyboardKeyEventArgs e)
		{
			// If not within, incorrect key or input is locked, don't do anything
			if (_proximity.Status != ProximityStatus.Within || e.Key != Key.E || _client.Player.LockInput) return;

			// Send teleport request to server
			var msg = _client.Structures.ObjectsNetwork.CreateMessageFor(this);
			_client.Network.SendToServer(msg, NetDeliveryMethod.ReliableUnordered, 0);
		}

		private void OnProximityChange(object s, ProximityEventArgs e)
		{
			if (e.NewStatus == ProximityStatus.Within)
			{
				_panel.Text = Text;
				_panel.VisibleCount++;
			}
			else
				_panel.VisibleCount--;
		}

		public override void Render()
		{
			var size = _type.Texture.Width/32f;
			_type.Texture.Render(Position - new Vector2(size * 0.5f), new Vector2(size));
		}
	}
}