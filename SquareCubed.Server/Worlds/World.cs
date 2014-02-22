using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using SquareCubed.Server.Players;

namespace SquareCubed.Server.Worlds
{
	public class World
	{
		private readonly List<Player> _players = new List<Player>();

		private readonly Server _server;

		public World(Server server)
		{
			_server = server;
		}

		public IEnumerable<Player> Players
		{
			get { return _players.AsReadOnly(); }
		}

		public void UpdatePlayerEntry(Player player)
		{
			if (player.Unit.World == this && !_players.Contains(player))
				_players.Add(player);
			else
				_players.Remove(player);
		}

		public void SendToAllPlayers(NetOutgoingMessage msg, NetDeliveryMethod method, int sequenceChannel = -1)
		{
			_server.Network.Peer.SendMessage(
				msg,
				Players.Select(p => p.Connection).ToList(),
				method,
				sequenceChannel);
		}
	}
}