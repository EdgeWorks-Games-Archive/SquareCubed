using System.Collections.Generic;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Server.Players
{
	public class Players
	{
		private readonly Logger _logger = new Logger("Players");
		private readonly PlayersNetwork _network;
		private readonly Dictionary<NetConnection, Player> _players = new Dictionary<NetConnection, Player>();
		private readonly Server _server;
		private uint _iterator = 1;

		public Players(Server server)
		{
			_server = server;
			_network = new PlayersNetwork(_server, this);
			_server.Meta.ClientDataReceived += OnClientDataReceived;
		}

		private void OnClientDataReceived(object sender, NetConnection con)
		{
			// Create the Player and the Player Unit we'll need
			var unit = new PlayerUnit
			{
				World = _server.Worlds.TestWorld,
				Position = new Vector2(0, 0)
			};
			var name = "Player " + _iterator++;
			var player = new Player(con, name, unit);

			// Make sure the player knows the existing structures and units
			_server.Structures.SendStructureDataFor(player);
			_server.Units.SendUnitDataFor(player);

			// Add the Player and the Player Unit to their collections and send the data
			_server.Units.Add(unit);
			_players.Add(con, player);
			_network.SendPlayerData(player);

			// And log it
			_logger.LogInfo("New player \"{0}\" added!", name);
		}

		public void OnPlayerPhysics(NetConnection con, Vector2 position)
		{
			_players[con].Unit.Position = position;
		}
	}
}