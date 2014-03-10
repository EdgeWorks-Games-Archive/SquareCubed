using OpenTK;

namespace SquareCubed.Common.Data
{
	/// <summary>
	///     Axis Aligned Line
	/// </summary>
	public class AaSide
	{
		/// <summary>
		///     The bottom/left position of the line.
		/// </summary>
		public Vector2 Position { get; set; }

		public float Length { get; set; }

		/// <summary>
		///     Distance this side is from the center of the AaBb.
		/// </summary>
		public float CenterDistance { get; set; }
	}
}