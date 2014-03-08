using System;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Data;

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
			client.Network.PacketHandlers.Bind("units.physics", OnUnitPhysics);
			client.Network.PacketHandlers.Bind("units.data", OnUnitData);
		}

		private void OnUnitPhysics(object s, NetIncomingMessage msg)
		{
			// Skip the packet type Id
			msg.ReadUInt16();
			msg.SkipPadBits();

			// Read the data
			var key = msg.ReadUInt32();
			var position = new Vector2(
				msg.ReadFloat(),
				msg.ReadFloat());

			// Pass the data on
			_callback.OnUnitPhysics(key, position);
		}

		private void OnUnitData(object s, NetIncomingMessage msg)
		{
			// Skip the packet type Id
			msg.ReadUInt16();
			msg.SkipPadBits();

			// Read the data
			var unit = new Unit(msg.ReadUInt32())
			{
				Position = msg.ReadVector2(),
				Structure = _client.Structures.GetOrNull(msg.ReadUInt32())
			};

			// Pass the data on
			_callback.OnUnitData(unit);
		}
	}
}