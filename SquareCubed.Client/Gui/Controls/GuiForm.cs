using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui.Controls
{
	public abstract class GuiForm : GuiControl
	{
		private const int TitleBarSize = 20 + 4 + 2; // Text height + padding + border
		private string _text;
		private Texture2D _textTexture;

		public Point Position { get; set; }

		public Size Size
		{
			// Size + 1px Border (x2) + Title Bar
			get { return InnerSize + new Size(2, 2 + TitleBarSize); }
			set { InnerSize = value - new Size(2, 2 + TitleBarSize); }
		}

		public string Title
		{
			get { return _text; }
			set
			{
				_text = value;
				if (_textTexture != null) _textTexture.Dispose();
				_textTexture = Texture2D.FromText(value, 14, Color.White); // 210, 210, 210
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
			GL.Vertex2(1, 1 + InnerSize.Height + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + InnerSize.Height + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1);

			// Title Bar Border
			GL.Color3(Color.FromArgb(0, 114, 198));
			GL.Vertex2(1, 1 + TitleBarSize - 2);
			GL.Vertex2(1, 1 + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + TitleBarSize - 2);

			GL.End();

			// Title Text
			_textTexture.Render(
				new Vector2(1 + 3, 1 + 2 + _textTexture.Height),
				new Vector2(_textTexture.Width, -_textTexture.Height));

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