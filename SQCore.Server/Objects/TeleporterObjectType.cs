using System;
using System.Collections.Generic;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Structures.Objects;

namespace SQCore.Server.Objects
{
	internal sealed class TeleporterObjectType : IServerObjectType
	{
		private readonly ObjectsNetwork _network;
		private readonly Players _players;
		private readonly Random _random = new Random();
		private readonly List<TeleporterObject> _teleporters = new List<TeleporterObject>();

		public TeleporterObjectType(ObjectsNetwork network, Players players)
		{
			_network = network;
			_players = players;
		}

		public ServerObjectBase CreateNew(ServerStructure parent)
		{
			return new TeleporterObject(this, _network, _players, parent);
		}

		public TeleporterObject GetRandomTeleporter(TeleporterObject exclude)
		{
			TeleporterObject teleporter;

			do teleporter = _teleporters[_random.Next(0, _teleporters.Count)]; while (teleporter == exclude);

			return teleporter;
		}

		public void AddTeleporter(TeleporterObject teleporter)
		{
			_teleporters.Add(teleporter);
		}
	}
}