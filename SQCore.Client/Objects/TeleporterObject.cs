using Lidgren.Network;
using OpenTK;
using OpenTK.Input;
using SquareCubed.Client;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class TeleporterObject : ClientObjectBase
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ProximityHelper _proximity;
		private readonly TeleporterObjectType _type;

		public TeleporterObject(ClientStructure parent, TeleporterObjectType type, SquareCubed.Client.Client client)
			: base(parent)
		{
			_type = type;
			_client = client;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new ProximityHelper(this, 0.5f);
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

		public override void Render()
		{
			var size = _type.Texture.Width/32f;
			_type.Texture.Render(Position - new Vector2(size * 0.5f), new Vector2(size));
		}
	}
}