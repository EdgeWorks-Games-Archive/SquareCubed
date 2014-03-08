using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
		private readonly Random _random = new Random();
		private readonly Server _server;
		private readonly List<ISpawnProvider> _spawnProviders = new List<ISpawnProvider>();
		private uint _iterator = 1;

		public Players(Server server)
		{
			Contract.Requires<ArgumentNullException>(server != null);

			_server = server;
			_network = new PlayersNetwork(_server, this);

			_server.Meta.ClientDataReceived += OnClientDataReceived;
			_server.Network.LostConnection += OnLostConnection;
		}

		/// <summary>
		///     Adds a new spawn provider. Spawn providers provide points
		///     for players to spawn at. A spawn provider might give the
		///     player a starting home in a station, or a starting ship.
		///     A spawn provider is randomly picked from the list when a
		///     new player connects to the server.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public void AddSpawnProvider(ISpawnProvider provider)
		{
			_spawnProviders.Add(provider);
		}

		private void OnClientDataReceived(object sender, NetConnection con)
		{
			// Make a random spawn provider provide us with a spawn
			var spawn = _spawnProviders[_random.Next(0, _spawnProviders.Count - 1)].GetNewSpawn();

			// Create the Player and the Player Unit we'll need
			var unit = new PlayerUnit
			{
				World = spawn.Structure.World,
				Structure = spawn.Structure,
				Position = spawn.Position
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

		private void OnLostConnection(object sender, NetIncomingMessage msg)
		{
			// If it wasn't even a player, don't bother doing anything
			if (!_players.ContainsKey(msg.SenderConnection)) return;

			// Clean up Player Data
			var player = _players[msg.SenderConnection];
			_server.Units.Remove(player.Unit);

			// Remove and Log
			_players.Remove(msg.SenderConnection);
			_logger.LogInfo("Player \"{0}\" removed!", player.Name);
		}
	}
}