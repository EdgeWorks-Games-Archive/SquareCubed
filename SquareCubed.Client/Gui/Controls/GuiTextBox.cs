using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Gui.Controls
{
	internal class GuiTextBox : GuiControl
	{
		private const int VerticalPadding = 4, HeightBottomCorrection = 2;
		private readonly GuiLabel _internalLabel;

		public GuiTextBox(string initialText = "", int fontSize = 12)
		{
			_internalLabel = new GuiLabel(initialText, fontSize);
		}

		public int Padding { get; set; }
		public int Width { get; set; }

		public int Height
		{
			get { return _internalLabel.Size.Height + (VerticalPadding*2) + HeightBottomCorrection; }
		}

		public Size Size
		{
			get { return new Size(Width, Height); }
		}

		public override void Render()
		{
			GL.Begin(PrimitiveType.Quads);

			// Input box
			GL.Color3(Color.FromArgb(45, 45, 45));
			GL.Vertex2(Position.X, Position.Y);
			GL.Vertex2(Position.X, Position.Y + Height);
			GL.Vertex2(Position.X + Width, Position.Y + Height);
			GL.Vertex2(Position.X + Width, Position.Y);

			GL.End();

			_internalLabel.Position = new Point(Padding + Position.X, Position.Y + VerticalPadding);
			_internalLabel.Render();
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				_internalLabel.Dispose();
			}

			base.Dispose(managed);
		}
	}
}