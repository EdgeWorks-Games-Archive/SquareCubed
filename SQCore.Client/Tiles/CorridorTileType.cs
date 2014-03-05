using SquareCubed.Client.Graphics;
using SquareCubed.Client.Tiles;

namespace SQCore.Client.Tiles
{
	class CorridorTileType : TileType
	{
		public CorridorTileType(SquareCubed.Client.Tiles.Tiles tiles)
		{
			Texture = new Texture2D("./Graphics/Tiles/Corridor.png");
			tiles.RegisterType(this, 2);
		}
	}
}