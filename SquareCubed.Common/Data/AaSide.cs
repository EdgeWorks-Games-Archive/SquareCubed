namespace SquareCubed.Common.Data
{
	/// <summary>
	///     Axis Aligned Line
	/// </summary>
	public class AaSide
	{
		/// <summary>
		///     The start position of this side on the axis it is aligned to.
		/// </summary>
		public float Start { get; set; }

		/// <summary>
		///     The end position of this side on the axis it is aligned to.
		/// </summary>
		public float End { get; set; }

		/// <summary>
		///     The position of this side on the axis tangential to the axis it is aligned to.
		/// </summary>
		public float Tangent { get; set; }

		/// <summary>
		///     The position of the AaBb's center on the axis tangential to the axis this side is aligned to.
		/// </summary>
		public float CenterTangent { get; set; }
	}
}