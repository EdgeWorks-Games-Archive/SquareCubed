using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
		}

		public ParentLink<World, Unit>.ChildrenCollection Units { get; private set; }

		#region Quick Lookup Collections

		private readonly List<Player> _players = new List<Player>();
		private readonly List<ServerStructure> _structures = new List<ServerStructure>();

		public IReadOnlyCollection<Player> Players
		{
			get { return _players.AsReadOnly(); }
		}

		public IReadOnlyCollection<ServerStructure> Structures
		{
			get { return _structures.AsReadOnly(); }
		}

		private void UpdateEntry<T>(ICollection<T> list, T entry, World newWorld)
		{
			// If this world, add, if not, remove
			if (newWorld == this)
			{
				// Make sure it's not already in this world before adding
				if (!list.Contains(entry))
					list.Add(entry);
			}
			else
				list.Remove(entry);
		}

		public void UpdatePlayerEntry(Player player)
		{
			Contract.Requires<ArgumentNullException>(player != null);
			UpdateEntry(_players, player, player.Unit.World);
		}

		public void UpdateStructureEntry(ServerStructure structure)
		{
			Contract.Requires<ArgumentNullException>(structure != null);
			UpdateEntry(_structures, structure, structure.World);
		}

		#endregion

		public void SendToAllPlayers(NetOutgoingMessage msg, NetDeliveryMethod method, int sequenceChannel = -1)
		{
			// If no players, don't bother at all
			if (_players.Count == 0) return;

			// Otherwise, send the data
			_server.Network.Peer.SendMessage(
				msg,
				Players.Select(p => p.Connection).ToList(),
				method,
				sequenceChannel);
		}
	}
}