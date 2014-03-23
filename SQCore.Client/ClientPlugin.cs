using SQCore.Client.Objects;
using SQCore.Client.Tiles;
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

			// Add tile types
			client.Structures.TileTypes.RegisterType(new CorridorTileType(), 2);
			client.Structures.TileTypes.RegisterType(new MetalFloorTileType(), 3);

			// Add object types
			client.Structures.ObjectTypes.RegisterType(typeof (PilotSeatObject), 0);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}