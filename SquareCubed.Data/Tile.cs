namespace SquareCubed.Data
{
	public class Tile
	{
		public enum WallSides
		{
			Top,
			Right,
			Bottom,
			Left
		}

		public Tile()
		{
			WallTypes = new uint[4];
		}

		public uint Type { get; set; }
		public uint[] WallTypes { get; set; }
	}
}