using SQCore.Client.Tiles;
using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		private StarsBackground _stars;

		private CorridorTileType _corridorTile;
		private MetalFloorTileType _metalFloorTile;

		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			_stars = new StarsBackground(client);
			_corridorTile = new CorridorTileType(client.Structures.TileTypes);
			_metalFloorTile = new MetalFloorTileType(client.Structures.TileTypes);

			Logger.LogInfo("Finished initializing core plugin!");
		}
	}
}