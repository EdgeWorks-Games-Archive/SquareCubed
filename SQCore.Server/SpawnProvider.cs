using OpenTK;
using SquareCubed.Common.Utils;
using SquareCubed.Server.Players;
using SquareCubed.Server.Structures;

namespace SQCore.Server
{
	internal class SpawnProvider : ISpawnProvider
	{
		private readonly Logger _logger;
		private readonly SquareCubed.Server.Server _server;
		private float _nextPosition;

		public SpawnProvider(SquareCubed.Server.Server server, Logger logger)
		{
			_server = server;
			_logger = logger;
		}

		public SpawnPoint GetNewSpawn()
		{
			// Create a test ship
			var str = new ServerStructure(new Vector2(6, 6))
			{
				World = _server.Worlds.TestWorld,
				Position = new Vector2(_nextPosition, 0)
			};
			_nextPosition += 4.0f;

			// Add a chunk for the test ship
			var chunk = new ServerChunk();
			str.Chunks.Add(chunk);

			// Cockpit
			chunk.SetTile(6, 7, 3);
			chunk.SetWalls(6, 7, 2, 2, 0, 2);
			// Add a pilot seat
			str.AddObject(6.5f, 7.5f, 0, _server.Structures.ObjectTypes);

			// Airlock
			chunk.SetTile(5, 6, 3);
			chunk.SetLeftWall(5, 6, 2);
			chunk.SetTopWall(5, 6, 2);
			chunk.SetBottomWall(5, 6, 2);
			str.AddObject(5.5f, 6.5f, 1, _server.Structures.ObjectTypes);

			// Hallway
			chunk.SetTile(6, 6, 2);
			chunk.SetRightWall(6, 6, 2);

			chunk.SetTile(6, 5, 2);
			chunk.SetRightWall(6, 5, 2);

			chunk.SetTile(6, 4, 2);
			chunk.SetRightWall(6, 4, 2);
			chunk.SetBottomWall(6, 4, 2);

			// Cargo Space
			chunk.SetTile(5, 5, 3);
			chunk.SetLeftWall(5, 5, 2);

			chunk.SetTile(5, 4, 3);
			chunk.SetLeftWall(5, 4, 2);
			chunk.SetBottomWall(5, 4, 2);
			chunk.SetRightWall(5, 4, 2);

			_logger.LogInfo("Spawned beginner ship at {0}, {1}!", str.Position.X, str.Position.Y);

			// Add the structure, this sends over the structure data
			// TODO: Perhaps do something about that not being obvious?
			// Perhaps remember that the structure has changed and only
			// send the changes later in the frame.
			_server.Structures.Add(str);

			// Now actually create the struct to describe where the player is
			return new SpawnPoint
			{
				Position = new Vector2(6.5f, 6.0f),
				Structure = str
			};
		}
	}
}