using OpenTK;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Structures.Tiles
{
	public abstract class TileType
	{
		protected static Vector2 Size = new Vector2(1, 1);
		protected Texture2D Texture;

		public virtual void RenderTile(Vector2 position)
		{
			// Render the tile's texture
			Texture.Render(position, Size);
		}
	}
}
