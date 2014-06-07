using SquareCubed.Client.Graphics;
using SquareCubed.Client.Structures.Tiles;

namespace SQCore.Client.Tiles
{
	class CorridorTileType : TileType
	{
		public CorridorTileType()
		{
			Texture = new Texture2D("./Graphics/Tiles/Corridor.png");
		}
	}
}