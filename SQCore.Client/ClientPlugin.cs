using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			client.UpdateTick += OnUpdate;

			Logger.LogInfo("Finished initializing core plugin!");
		}

		private void OnUpdate(object sender, float delta)
		{
		}
	}
}