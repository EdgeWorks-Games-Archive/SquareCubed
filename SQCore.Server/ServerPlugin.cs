using SQCore.Common;
using SquareCubed.Server;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			// Add the default spawn provider
			server.Players.AddSpawnProvider(new SpawnProvider(server, Logger));

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}