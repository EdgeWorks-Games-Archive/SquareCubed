using SquareCubed.Client.Graphics;
using SquareCubed.Client.Tiles;

namespace SQCore.Client.Tiles
{
	class MetalFloorTileType : TileType
	{
		public MetalFloorTileType(SquareCubed.Client.Tiles.Tiles tiles)
		{
			Texture = new Texture2D("./Graphics/Tiles/MetalFloor.png");
			tiles.RegisterType(this, 3);
		}
	}
}