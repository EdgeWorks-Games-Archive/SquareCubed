using OpenTK;
using SQCore.Common;
using SquareCubed.Data;
using SquareCubed.Server;
using SquareCubed.Server.Structures;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			// Create a test ship
			var str = new Structure
			{
				World = server.Worlds.TestWorld,
				Position = new Vector2(-5.5f, -5.5f)
			};

			// Add a chunk for the test ship
			var chunk = new Chunk();

			// Cockpit
			chunk.Tiles[6][7] = new Tile {Type = 3};
			chunk.Tiles[6][7].WallTypes[(int) WallSides.Top] = 2;
			chunk.Tiles[6][7].WallTypes[(int) WallSides.Right] = 2;
			chunk.Tiles[5][7] = new Tile {Type = 0};
			chunk.Tiles[5][7].WallTypes[(int) WallSides.Right] = 2;

			// Top Left
			chunk.Tiles[5][6] = new Tile {Type = 3};
			chunk.Tiles[5][6].WallTypes[(int) WallSides.Top] = 2;

			// Top Right
			chunk.Tiles[6][6] = new Tile {Type = 2};
			chunk.Tiles[6][6].WallTypes[(int) WallSides.Right] = 2;

			// Middle Left
			chunk.Tiles[5][5] = new Tile {Type = 3};
			chunk.Tiles[5][5].WallTypes[(int) WallSides.Top] = 2;
			chunk.Tiles[4][5] = new Tile {Type = 0};
			chunk.Tiles[4][5].WallTypes[(int) WallSides.Right] = 2;

			// Middle Right
			chunk.Tiles[6][5] = new Tile {Type = 2};
			chunk.Tiles[6][5].WallTypes[(int) WallSides.Right] = 2;

			// Bottom Left
			chunk.Tiles[5][4] = new Tile {Type = 3};
			chunk.Tiles[5][4].WallTypes[(int) WallSides.Right] = 2;
			chunk.Tiles[4][4] = new Tile {Type = 0};
			chunk.Tiles[4][4].WallTypes[(int) WallSides.Right] = 2;
			chunk.Tiles[5][3] = new Tile {Type = 0};
			chunk.Tiles[5][3].WallTypes[(int) WallSides.Top] = 2;

			// Bottom Right
			chunk.Tiles[6][4] = new Tile {Type = 2};
			chunk.Tiles[6][4].WallTypes[(int) WallSides.Right] = 2;
			chunk.Tiles[6][3] = new Tile {Type = 0};
			chunk.Tiles[6][3].WallTypes[(int) WallSides.Top] = 2;

			str.Chunks.Add(chunk);

			// And add the ship to the collection
			server.Structures.Add(str);
			Logger.LogInfo("Spawned test ship at {0}, {1}!", str.Position.X, str.Position.Y);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}