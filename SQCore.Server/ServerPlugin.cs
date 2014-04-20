using OpenTK;
using SQCore.Common;
using SquareCubed.Server;
using SquareCubed.Server.Structures;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			// Add the default spawn provider
			server.Players.AddSpawnProvider(new SpawnProvider(server, Logger));

			// Add a little test structure
			var str = new Structure
			{
				World = server.Worlds.TestWorld,
				Position = new Vector2(3, 2),
				Rotation = 0.0f,
				Center = new Vector2(5.5f, 5.5f)
			};

			// Add a chunk for the test structure
			var chunk = new ServerChunk();
			chunk.SetTile(5, 5, 3);
			chunk.SetWalls(5, 5, 2, 2, 2, 2);
			str.Chunks.Add(chunk);

			// Add the structure to the world
			server.Structures.Add(str);

			Logger.LogInfo("Finished initializing core plugin!");
		}

		public void Dispose()
		{
		}
	}
}