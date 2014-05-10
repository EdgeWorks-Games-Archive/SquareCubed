using OpenTK;

namespace SquareCubed.Client.Structures.Tiles
{
	class InvisibleTileType : TileType
	{
		public override void RenderTile(Vector2 position)
		{
			// Invisible TileTypes of course do not render
		}
	}
}
