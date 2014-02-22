using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Server.Players
{
	internal class PlayersNetwork
	{
		private readonly Server _server;
		private readonly ushort _packetType;

		public PlayersNetwork(Server server)
		{
			_server = server;
			_packetType = _server.Network.PacketHandlers.ResolveType("players.data");
		}

		public void SendPlayerData(Player player)
		{
			var msg = _server.Network.Peer.CreateMessage();

			// Add the packet type Id
			msg.Write(_packetType);
			msg.WritePadBits();

			// Send over unit Id so client can link it to the player
			msg.Write(player.Unit.Id);

			player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
		}
	}
}