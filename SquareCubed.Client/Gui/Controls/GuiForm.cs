using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui.Controls
{
	public abstract class GuiForm : GuiControl
	{
		private string _text;
		private Texture2D _textTexture;

		public Point Position { get; set; }

		public Size Size
		{
			// Size + 1px Border (x2)
			get { return InnerSize + new Size(2, 2); }
			set { InnerSize = value - new Size(2, 2); }
		}

		public string Title
		{
			get { return _text; }
			set
			{
				_text = value;
				if (_textTexture != null) _textTexture.Dispose();
				_textTexture = Texture2D.FromText(value, 10, new SolidBrush(Color.White)); // 210, 210, 210
			}
		}

		public Size InnerSize { get; set; }

		public override void Render()
		{
			GL.PushMatrix();
			GL.Translate(Position.X, Position.Y, 0);

			GL.Begin(PrimitiveType.Quads);

			// Border
			GL.Color3(Color.FromArgb(40, 40, 40));
			GL.Vertex2(0, 0);
			GL.Vertex2(0, Size.Height);
			GL.Vertex2(Size.Width, Size.Height);
			GL.Vertex2(Size.Width, 0);

			// Center
			GL.Color3(Color.FromArgb(50, 50, 50));
			GL.Vertex2(1, 1);
			GL.Vertex2(1, 1 + InnerSize.Height);
			GL.Vertex2(1 + InnerSize.Width, 1 + InnerSize.Height);
			GL.Vertex2(1 + InnerSize.Width, 1);

			GL.End();


			GL.PopMatrix();
		}

		protected override void Dispose(bool managed)
		{
			if (!managed)
				return;

			_textTexture.Dispose();
		}
	}
}