using SQCore.Client.Stars;
using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		private StarsBackground _stars;

		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			_stars = new StarsBackground(client);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}