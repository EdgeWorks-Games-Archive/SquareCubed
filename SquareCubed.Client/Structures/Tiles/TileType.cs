using System;
using OpenTK;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Structures.Tiles
{
	public abstract class TileType : IDisposable
	{
		protected static Vector2 Size = new Vector2(1, 1);
		protected Texture2D Texture;

		public virtual void RenderTile(Vector2 position)
		{
			// Render the tile's texture
			Texture.Render(position, Size);
		}

		~TileType()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool managed)
		{
			if (!managed) return;

			// A texture can be null if a class inheriting this doesn't need the basic tile rendering
			if(Texture != null) Texture.Dispose();
		}
	}
}
