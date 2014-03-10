using OpenTK;

namespace SquareCubed.Common.Data
{
	/// <summary>
	///     Axis Aligned Bounding Box
	/// </summary>
	public class AaBb
	{
		/// <summary>
		///     The bottom/left position of the box.
		/// </summary>
		public Vector2 Position { get; set; }

		/// <summary>
		///     The size of the box.
		/// </summary>
		public Vector2 Size { get; set; }

		public AaSide Right
		{
			get
			{
				return new AaSide
				{
					Position = new Vector2(Position.X + Size.X, Position.Y),
					Length = Size.Y,
					CenterDistance = Size.X * 0.5f
				};
			}
		}
	}
}