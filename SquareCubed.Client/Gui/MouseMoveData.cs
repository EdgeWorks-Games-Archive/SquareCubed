using System.Drawing;

namespace SquareCubed.Client.Gui
{
	public class MouseMoveData
	{
		public Point Position { get; private set; }
		public Point PreviousPosition { get; private set; }

		public MouseMoveData(Point position, Point previousPosition)
		{
			Position = position;
			PreviousPosition = previousPosition;
		}
	}
}