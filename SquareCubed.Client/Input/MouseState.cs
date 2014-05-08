using OpenTK;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Input
{
	public class MouseState
	{
		/// <summary>
		///     Position of the mouse relative to the window.
		/// </summary>
		public Vector2i AbsolutePosition { get; set; }

		/// <summary>
		///     Position of the mouse relative to the parent. (World or Structure)
		/// </summary>
		public Vector2 RelativePosition { get; set; }
	}
}