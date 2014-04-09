using SQCore.Client.Background;
using SQCore.Client.Objects;
using SQCore.Client.Tiles;
using SQCore.Common;
using SquareCubed.Client;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Tiles;

namespace SQCore.Client
{
	public sealed class ClientPlugin : CommonPlugin, IClientPlugin
	{
		#region Tile Types

		private readonly CorridorTileType _corridorTile;
		private readonly MetalFloorTileType _metalFloorTile;

		#endregion

		private readonly Chat.Chat _chat;
		private readonly ObjectTypes _objectTypes;
		private readonly Space _stars;
		private readonly TileTypes _tileTypes;
		private readonly SquareCubed.Client.Client _client;

		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			_client = client;
			_tileTypes = _client.Structures.TileTypes;
			_objectTypes = _client.Structures.ObjectTypes;

			_stars = new Space(_client.Graphics.Camera.Resolution, new SpaceRenderer(_client.Graphics.Camera));
			_stars.GenerateStars();

			_chat = new Chat.Chat(_client.Gui);

			// Add tile types
			_corridorTile = new CorridorTileType();
			_tileTypes.RegisterType(_corridorTile, 2);
			_metalFloorTile = new MetalFloorTileType();
			_tileTypes.RegisterType(_metalFloorTile, 3);

			// Add object types
			_objectTypes.RegisterType(typeof (PilotSeatObject), 0);

			// Bind events
			_client.BackgroundRenderTick += RenderBackground;

			Logger.LogInfo("Finished initializing core plugin!");
		}

		public void Dispose()
		{
			// Remove tile types
			_tileTypes.UnregisterType(_corridorTile);
			_tileTypes.UnregisterType(_metalFloorTile);

			// Remove object types
			_objectTypes.UnregisterType(typeof (PilotSeatObject));

			// Unbind events
			_client.BackgroundRenderTick -= RenderBackground;
		}

		private void RenderBackground(object sender, TickEventArgs e)
		{
			_stars.Render();
		}
	}
}