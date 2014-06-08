using Lidgren.Network;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;
using SquareCubed.Network;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures.Objects;

namespace SquareCubed.Server.Structures
{
	internal class StructuresNetwork
	{
		private readonly PacketType _dataPacketType;
		private readonly Network.Network _network;
		private readonly PacketType _physicsPacketType;

		public StructuresNetwork(Network.Network network)
		{
			_network = network;
			_physicsPacketType = _network.PacketTypes.ResolveType("structures.physics");
			_dataPacketType = _network.PacketTypes.ResolveType("structures.data");
		}

		public void SendStructurePhysics(ServerStructure structure)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_physicsPacketType);

			// Add data
			msg.Write(structure.Id);
			msg.Write(structure.Position);
			msg.Write(structure.Body.Rotation);

			// Send data to appropriate players
			structure.World.SendToAllPlayers(msg, NetDeliveryMethod.UnreliableSequenced, (int) SequenceChannels.StructurePhysics);
		}

		public void SendStructureData(ServerStructure structure, TypeRegistry<IServerObjectType> types, Player player = null)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_dataPacketType);

			// And add the data
			msg.Write(structure, types);

			if (player != null)
			{
				// Send the data to the player
				player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.StructureData);
			}
			else
			{
				// Send the data to all players in the world the structure is in
				structure.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.StructureData);
			}
		}
	}
}