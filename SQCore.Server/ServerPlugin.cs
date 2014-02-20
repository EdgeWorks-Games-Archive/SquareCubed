using SQCore.Common;
using SquareCubed.Server;

namespace SQCore.Server
{
	public class ServerPlugin : CommonPlugin, IServerPlugin
	{
		private float _accumulator;
		private SquareCubed.Server.Server _server;

		public ServerPlugin(SquareCubed.Server.Server server)
		{
			Logger.LogInfo("Initializing core plugin...");

			_server = server;
			_server.UpdateTick += OnUpdate;

			Logger.LogInfo("Finished initializing core plugin!");
		}

		private void OnUpdate(object sender, float delta)
		{
			if (_accumulator > 10)
			{
				Logger.LogInfo("10 Second Tick");
				_accumulator = 0;
			}
			else _accumulator += delta;
		}
	}
}