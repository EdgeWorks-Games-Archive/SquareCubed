using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");
			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}