using OpenTK;

namespace SquareCubed.Client.Structures.Tiles
{
	class InvisibleTileType : TileType
	{
		public InvisibleTileType(SquareCubed.Client.Structures.Tiles.TileTypes tileTypes)
		{
			tileTypes.RegisterType(this, 1);
		}

		public override void RenderTile(Vector2 position)
		{
			// Invisible TileTypes of course do not render
		}
	}
}
