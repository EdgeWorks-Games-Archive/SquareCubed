using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		private SquareCubed.Client.Client _client;

		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			_client = client;

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}