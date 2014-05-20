using System;
using Lidgren.Network;
using SquareCubed.Common.Data;
using SquareCubed.Network;
using SquareCubed.Server.Players;

namespace SquareCubed.Server.Units
{
	internal class UnitsNetwork
	{
		private readonly PacketType _dataPacketType;
		private readonly Network.Network _network;
		private readonly PacketType _physicsPacketType;
		private readonly PacketType _teleportPacketType;

		public UnitsNetwork(Network.Network network)
		{
			_network = network;
			_physicsPacketType = _network.PacketTypes.ResolveType("units.physics");
			_dataPacketType = _network.PacketTypes.ResolveType("units.data");
			_teleportPacketType = _network.PacketTypes.ResolveType("units.teleport");
		}

		public void SendUnitPhysics(Unit unit)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_physicsPacketType);

			// The client knows what unit to update/create using the Id
			msg.Write(unit.Id);

			// Add data
			msg.Write(unit.Position);

			// Send data to appropriate players
			unit.World.SendToAllPlayers(msg, NetDeliveryMethod.UnreliableSequenced, (int) SequenceChannels.UnitPhysics);
		}

		public void SendUnitData(Unit unit, Player player = null)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_dataPacketType);

			// The client knows what unit to update using the Id
			msg.Write(unit.Id);

			// Add data
			msg.Write(unit.Position);
			msg.Write(unit.Structure.Id);

			if (player != null)
			{
				// Send the data to the player
				player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
			}
			else
			{
				// Send the data to all players in the world the unit is in
				unit.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
			}
		}

		public void SendTeleport(Unit unit)
		{
			var msg = _network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_teleportPacketType);

			// The client knows what unit to update using the Id
			msg.Write(unit.Id);

			// Add data
			msg.Write(unit.Position);
			msg.Write(unit.Structure.Id);

			// Send data to appropriate players
			unit.World.SendToAllPlayers(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitTeleport);
		}
	}
}