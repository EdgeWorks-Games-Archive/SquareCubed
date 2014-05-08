namespace SquareCubed.Common.Data
{
	public struct Vector2i
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2i(int x, int y)
			:this()
		{
			X = x;
			Y = y;
		}
	}
}