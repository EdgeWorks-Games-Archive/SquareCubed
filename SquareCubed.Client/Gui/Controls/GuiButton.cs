using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Client.Gui.Controls
{
	internal class GuiButton : GuiControl
	{
		private readonly GuiLabel _internalLabel;

		public GuiButton(string text, int fontSize = 13)
		{
			_internalLabel = new GuiLabel(text, fontSize)
			{
				Color = EngineColors.InputText
			};
		}

		public override Size Size { get; set; }

		public override void Render()
		{
			GL.Begin(PrimitiveType.Quads);

			// Background
			GL.Color3(IsHovered ? EngineColors.ButtonHoverBackground : EngineColors.ButtonBackground);
			GL.Vertex2(Position.X, Position.Y);
			GL.Vertex2(Position.X, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y);

			// Border
			GL.Color3(EngineColors.Border);
			GL.Vertex2(Position.X, Position.Y + Size.Height - 1);
			GL.Vertex2(Position.X, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y + Size.Height);
			GL.Vertex2(Position.X + Size.Width, Position.Y + Size.Height - 1);

			GL.End();

			// Internal Text
			_internalLabel.Position = new Point(
				Position.X + (Size.Width/2) - (_internalLabel.Size.Width/2),
				Position.Y + ((Size.Height - 2)/2) - (_internalLabel.Size.Height/2));
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

		protected override void OnMouseClick(MousePressData data)
		{
			Console.WriteLine("Click!");

			base.OnMouseClick(data);
		}
	}
}