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

			// Add a single chunk with a single tile
			var chunk = new Chunk();
			chunk.Tiles[5][5] = new Tile
			{
				Type = 2
			};
			str.Chunks.Add(chunk);

			// And add the ship to the collection
			server.Structures.Add(str);
			Logger.LogInfo("Spawned test ship at {0}, {1}!", str.Position.X, str.Position.Y);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}