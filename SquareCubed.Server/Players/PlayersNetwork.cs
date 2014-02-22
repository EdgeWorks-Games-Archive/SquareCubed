using Lidgren.Network;
using SquareCubed.Network;

namespace SquareCubed.Server.Players
{
	internal class PlayersNetwork
	{
		private readonly Server _server;

		public PlayersNetwork(Server server)
		{
			_server = server;
		}

		public void SendPlayerData(Player player)
		{
			var msg = _server.Network.Peer.CreateMessage();

			// Add the packet type Id
			// TODO: loop up what the Id actually is instead of having it set here
			msg.Write((ushort)1);
			msg.WritePadBits();

			// Send over unit Id so client can link it to the player
			msg.Write(player.Unit.Id);

			player.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, (int) SequenceChannels.UnitData);
		}
	}
}