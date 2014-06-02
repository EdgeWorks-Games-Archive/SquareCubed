using System.Linq;
using Lidgren.Network;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Units;
using SquareCubed.Common.Utils;

namespace SquareCubed.Server.Worlds
{
	public class World
	{
		private readonly Server _server;

		public World(Server server)
		{
			_server = server;
			Units = new ParentLink<World, Unit>.ChildrenCollection(this, u => u.WorldLink);
			Players = new ParentLink<World, Player>.ChildrenCollection(this, p => p.WorldLink);
			Structures = new ParentLink<World, ServerStructure>.ChildrenCollection(this, s => s.WorldLink);
		}

		public ParentLink<World, Unit>.ChildrenCollection Units { get; private set; }
		public ParentLink<World, Player>.ChildrenCollection Players { get; private set; }
		public ParentLink<World, ServerStructure>.ChildrenCollection Structures { get; private set; }

		public void SendToAllPlayers(NetOutgoingMessage msg, NetDeliveryMethod method, int sequenceChannel = -1)
		{
			// If no players, don't bother at all
			if (Players.Count == 0) return;

			// Otherwise, send the data
			_server.Network.Peer.SendMessage(
				msg,
				Players.Select(p => p.Connection).ToList(),
				method,
				sequenceChannel);
		}
	}
}