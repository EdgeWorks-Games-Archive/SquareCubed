using OpenTK;

namespace SquareCubed.Data
{
	public class Chunk
	{
		public Chunk()
		{
			// Initialize tiles array
			Tiles = new Tile[16][];
			for (var i = 0; i < Tiles.Length; i++)
				Tiles[i] = new Tile[16];
		}

		public Tile[][] Tiles { get; private set; }

		#region Position

		// OpenTK doesn't have an int version of Vector2

		public int X { get; set; }
		public int Y { get; set; }

		#endregion
	}
}