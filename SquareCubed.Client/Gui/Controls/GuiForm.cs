using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui.Controls
{
	public abstract class GuiForm : GuiControl.GuiParentControl
	{
		private const int TitleBarSize = 20 + 4 + 2; // Text height + padding + border
		private readonly Size _innerOffset;
		private string _text;
		private Texture2D _textTexture;

		protected GuiForm(string title)
		{
			Debug.Assert(title != null);

			Title = title;
			_innerOffset = new Size(1, TitleBarSize + 1);
		}

		public override Size InnerOffset
		{
			get { return _innerOffset; }
		}

		public override Size Size
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
				Debug.Assert(value != null);

				_text = value;
				if (_textTexture != null) _textTexture.Dispose();
				_textTexture = TextHelper.RenderString(value, 14, EngineColors.Heading);
			}
		}

		public override Size InnerSize { get; set; }

		internal override void Render(float delta)
		{
			GL.PushMatrix();
			GL.Translate(Position.X, Position.Y, 0);

			GL.Begin(PrimitiveType.Quads);

			// Border
			GL.Color3(EngineColors.Border);
			GL.Vertex2(0, 0);
			GL.Vertex2(0, Size.Height);
			GL.Vertex2(Size.Width, Size.Height);
			GL.Vertex2(Size.Width, 0);

			// Center
			GL.Color3(EngineColors.Background);
			GL.Vertex2(1, 1);
			GL.Vertex2(1, 1 + InnerSize.Height + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + InnerSize.Height + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1);

			// Title Bar Border
			GL.Color3(EngineColors.Highlight);
			GL.Vertex2(1, 1 + TitleBarSize - 2);
			GL.Vertex2(1, 1 + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + TitleBarSize);
			GL.Vertex2(1 + InnerSize.Width, 1 + TitleBarSize - 2);

			GL.End();

			// Title Text
			_textTexture.Render(
				new Vector2(1 + 6, 1 + 2 + _textTexture.Height),
				new Vector2(_textTexture.Width, -_textTexture.Height));

			// Render all the children
			GL.Translate(1, 1 + TitleBarSize, 0);
			base.Render(delta);

			GL.PopMatrix();
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				_textTexture.Dispose();
			}

			base.Dispose(managed);
		}
	}
}