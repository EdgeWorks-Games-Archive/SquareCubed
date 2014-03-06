using Lidgren.Network;
using OpenTK;
using SquareCubed.Network;

namespace SquareCubed.Server.Players
{
	internal class PlayersNetwork
	{
		private readonly Players _callback;
		private readonly ushort _packetType;
		private readonly Server _server;

		public PlayersNetwork(Server server, Players callback)
		{
			_server = server;
			_callback = callback;
			_packetType = _server.Network.PacketHandlers.ResolveType("players.data");
			_server.Network.PacketHandlers.Bind(_packetType, OnPlayerPhysics);
		}

		private void OnPlayerPhysics(object s, NetIncomingMessage msg)
		{
			// Skip the packet type Id
			msg.ReadUInt16();
			msg.SkipPadBits();

			// Read the data
			var position = new Vector2(
				msg.ReadFloat(),
				msg.ReadFloat());

			// Pass the data on
			_callback.OnPlayerPhysics(msg.SenderConnection, position);
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