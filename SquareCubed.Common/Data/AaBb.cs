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
					Start = Position.Y,
					End = Position.Y + Size.Y,
					Tangent = Position.X + Size.X,
					CenterTangent = Position.X + (Size.X * 0.5f)
				};
			}
		}

		public AaSide Left
		{
			get
			{
				return new AaSide
				{
					Start = Position.Y,
					End = Position.Y + Size.Y,
					Tangent = Position.X,
					CenterTangent = Position.X + (Size.X * 0.5f)
				};
			}
		}

		public AaSide Up
		{
			get
			{
				return new AaSide()
				{
					Start = Position.X,
					End = Position.X + Size.X,
					Tangent = Position.Y + Size.Y,
					CenterTangent = Position.Y + (Size.Y*0.5f)
				};
			}
		}

		public AaSide Down
		{
			get
			{
				return new AaSide
				{
					Start = Position.X,
					End = Position.X + Size.X,
					Tangent = Position.Y,
					CenterTangent = Position.Y + (Size.Y * 0.5f)
				};
			}
		}
	}
}