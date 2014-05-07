using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Units
{
	internal class UnitsNetwork
	{
		private readonly Units _callback;
		private readonly Client _client;

		public UnitsNetwork(Client client, Units callback)
		{
			_client = client;
			_callback = callback;

			var network = _client.Network;
			network.PacketHandlers.Bind(network.PacketTypes["units.physics"], OnUnitPhysics);
			network.PacketHandlers.Bind(network.PacketTypes["units.data"], OnUnitData);
		}

		private void OnUnitPhysics(NetIncomingMessage msg)
		{
			// Read the data
			var key = msg.ReadInt32();
			var position = new Vector2(
				msg.ReadFloat(),
				msg.ReadFloat());

			// Pass the data on
			_callback.OnUnitPhysics(key, position);
		}

		private void OnUnitData(NetIncomingMessage msg)
		{
			// Read the data
			var unit = new Unit(msg.ReadInt32())
			{
				Position = msg.ReadVector2(),
				Structure = _client.Structures.GetOrNull(msg.ReadInt32())
			};

			// Pass the data on
			_callback.OnUnitData(unit);
		}
	}
}