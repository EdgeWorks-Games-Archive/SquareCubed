using Lidgren.Network;
using SquareCubed.Network;
using SquareCubed.Server.Players;

namespace SquareCubed.Server.Structures
{
	class StructuresNetwork
	{
		private readonly ushort _physicsPacketType;
		private readonly ushort _dataPacketType;
		private readonly Network.Network _network;

		public StructuresNetwork(Network.Network network)
		{
			_network = network;
			_physicsPacketType = _network.PacketHandlers.ResolveType("structures.physics");
			_dataPacketType = _network.PacketHandlers.ResolveType("structures.data");
		}

		public void SendStructurePhysics(Structure structure)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_physicsPacketType);
			msg.WritePadBits();

			// Add data
			msg.Write(structure.Id);
			msg.Write(structure.Position.X);
			msg.Write(structure.Position.Y);
			msg.Write(structure.Rotation);
			msg.Write(structure.Center.X);
			msg.Write(structure.Center.Y);

			// Send data to appropriate players
			structure.World.SendToAllPlayers(msg, NetDeliveryMethod.UnreliableSequenced, (int)SequenceChannels.UnitPhysics);
		}

		public void SendStructureData(Structure structure, Player player = null)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_dataPacketType);
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
				// Send the data to all players in the world the structure is in
				structure.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int)SequenceChannels.UnitData);
			}
		}
	}
}