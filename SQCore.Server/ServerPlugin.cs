using SQCore.Common;
using SquareCubed.Server;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			server.UpdateTick += OnUpdate;

			Logger.LogInfo("Finished initializing core plugin!");
		}

		private void OnUpdate(object sender, float delta)
		{
		}
	}
}