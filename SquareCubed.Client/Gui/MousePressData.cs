using System;
using System.Drawing;
using OpenTK.Input;

namespace SquareCubed.Client.Gui
{
	public class MousePressData
	{
		public MousePressData(Point position, MouseButton button, MousePressEndEvent evt)
		{
			Position = position;
			Button = button;
			EndEvent = evt;
		}

		public Point Position { get; private set; }
		public MouseButton Button { get; private set; }
		public MousePressEndEvent EndEvent { get; private set; }

		public class MousePressEndEvent
		{
			public event EventHandler Event = (s, e) => { };

			public void Invoke()
			{
				Event(this, EventArgs.Empty);
			}
		}
	}
}