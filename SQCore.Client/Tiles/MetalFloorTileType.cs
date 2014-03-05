using SquareCubed.Client.Graphics;
using SquareCubed.Client.Structures.Tiles;

namespace SQCore.Client.Tiles
{
	class MetalFloorTileType : TileType
	{
		public MetalFloorTileType(TileTypes tileTypes)
		{
			Texture = new Texture2D("./Graphics/Tiles/MetalFloor.png");
			tileTypes.RegisterType(this, 3);
		}
	}
}