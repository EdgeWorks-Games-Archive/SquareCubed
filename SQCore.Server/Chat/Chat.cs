using Lidgren.Network;
using SquareCubed.Common.Utils;
using SquareCubed.Network;
using SquareCubed.Server.Players;

namespace SQCore.Server.Chat
{
	class Chat
	{
		private readonly ChatNetwork _network;
		private readonly Players _players;
		private readonly Logger _logger = new Logger("Chat");

		public Chat(Network network, Players players)
		{
			_network = new ChatNetwork(network, this);
			_players = players;
		}

		public void Send(Player player, string message)
		{
			_logger.LogInfo("{0}: {1}", player.Name, message);
			_network.SendWorldChatMessage(player, message);
		}

		public void OnChatMessage(NetConnection connection, string message)
		{
			Send(_players[connection], message);
		}
	}
}
