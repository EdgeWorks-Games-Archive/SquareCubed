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
				Position = new Vector2(0, 0),
				Rotation = 21.0f,
				Center = new Vector2(6.0f, 6.0f)
			};
			
			// Add a chunk for the test ship
			var chunk = new Chunk();

			// Cockpit
			chunk.SetTile(6, 7, 3);
			chunk.SetWalls(6, 7, 2, 2, 0, 2);

			// Airlock
			chunk.SetTile(5, 6, 3);
			chunk.SetTopWall(5, 6, 2);
			chunk.SetBottomWall(5, 6, 2);

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

			str.Chunks.Add(chunk);

			// And add the ship to the collection
			server.Structures.Add(str);
			Logger.LogInfo("Spawned test ship at {0}, {1}!", str.Position.X, str.Position.Y);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}