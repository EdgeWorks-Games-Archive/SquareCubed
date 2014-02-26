using Lidgren.Network;
using SquareCubed.Network;
using SquareCubed.Server.Players;

namespace SquareCubed.Server.Structures
{
	class StructuresNetwork
	{
		private readonly ushort _packetType;
		private readonly Server _server;

		public StructuresNetwork(Server server)
		{
			_server = server;
			_packetType = _server.Network.PacketHandlers.ResolveType("structures.data");
		}

		public void SendStructureData(Structure structure, Player player = null)
		{
			var msg = _server.Network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_packetType);
			msg.WritePadBits();

			// And add the data
			msg.Write(structure);

			if (player != null)
			{
				// Send the data to the player
				player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int)SequenceChannels.UnitData);
			}
			else
			{
				// Send the data to all players in the world the unit is in
				structure.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int)SequenceChannels.UnitData);
			}
		}
	}
}