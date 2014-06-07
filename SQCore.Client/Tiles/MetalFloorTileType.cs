using SquareCubed.Client.Graphics;
using SquareCubed.Client.Structures.Tiles;

namespace SQCore.Client.Tiles
{
	class MetalFloorTileType : TileType
	{
		public MetalFloorTileType()
		{
			Texture = new Texture2D("./Graphics/Tiles/MetalFloor.png");
		}
	}
}