using OpenTK;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Tiles
{
	class InvisibleTileType : TileType
	{
		public InvisibleTileType(Tiles tiles)
		{
			Texture = new Texture2D("./Graphics/Tiles/MetalFloor.png");
			tiles.RegisterType(this, 1);
		}

		public override void RenderTile(Vector2 position)
		{
			// Invisible tiles of course do not render
		}
	}
}
