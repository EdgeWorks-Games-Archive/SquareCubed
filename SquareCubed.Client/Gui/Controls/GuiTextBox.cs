using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui.Controls
{
	internal class GuiTextBox : GuiControl
	{
		private const int VerticalPadding = 4, HeightBottomCorrection = 0;
		private readonly GuiLabel _internalLabel;
		private float _cursorTimer;

		public GuiTextBox(string initialText = "", int fontSize = 13)
		{
			_internalLabel = new GuiLabel(initialText, fontSize)
			{
				Color = EngineColors.InputText
			};
		}

		public int CursorPosition { get; set; }
		public int Padding { get; set; }
		public int Width { get; set; }

		public int Height
		{
			get { return _internalLabel.Size.Height + (VerticalPadding*2) + HeightBottomCorrection; }
		}

		public override Size Size
		{
			get { return new Size(Width, Height); }
			set { throw new ReadOnlyException(); }
		}

		public string Text
		{
			get { return _internalLabel.Text; }
		}

		internal override void Render(float delta)
		{
			GL.Begin(PrimitiveType.Quads);

			// Input Box
			GL.Color3(Color.FromArgb(45, 45, 45));
			GL.Vertex2(Position.X, Position.Y);
			GL.Vertex2(Position.X, Position.Y + Height);
			GL.Vertex2(Position.X + Width, Position.Y + Height);
			GL.Vertex2(Position.X + Width, Position.Y);

			GL.End();

			// Internal Text
			_internalLabel.Position = new Point(Position.X + Padding, Position.Y + VerticalPadding);
			_internalLabel.Render(delta);

			// Only display the cursor half of a second
			if (_cursorTimer%1.0f < 0.5f && IsFocused)
			{
				GL.Begin(PrimitiveType.Quads);

				// Text Cursor
				var offset = Position.X + Padding + TextHelper.MeasureString(
					_internalLabel.Text.Substring(0, CursorPosition),
					_internalLabel.FontSize).Width;
				GL.Color3(EngineColors.InputText);
				GL.Vertex2(offset, Position.Y + VerticalPadding);
				GL.Vertex2(offset, Position.Y + VerticalPadding + _internalLabel.Size.Height);
				GL.Vertex2(offset + 1, Position.Y + VerticalPadding + _internalLabel.Size.Height);
				GL.Vertex2(offset + 1, Position.Y + VerticalPadding);

				GL.End();
			}
			_cursorTimer += delta;
		}

		protected override void OnKeyChar(char key)
		{
			_internalLabel.Text = _internalLabel.Text.Insert(CursorPosition, key.ToString(CultureInfo.InvariantCulture));
			CursorPosition++;

			base.OnKeyChar(key);
		}

		protected override void OnKeyDown(Key key)
		{
			switch (key)
			{
				case Key.BackSpace:
					if (CursorPosition > 0)
					{
						_internalLabel.Text = _internalLabel.Text.Remove(CursorPosition - 1, 1);
						CursorPosition--;
					}
					break;
				case Key.Delete:
					if (CursorPosition < _internalLabel.Text.Length)
						_internalLabel.Text = _internalLabel.Text.Remove(CursorPosition, 1);
					break;

				case Key.Left:
					if(CursorPosition > 0)
						CursorPosition--;
					_cursorTimer = 0;
					break;
				case Key.Right:
					if (CursorPosition < _internalLabel.Text.Length)
						CursorPosition++;
					_cursorTimer = 0;
					break;
			}

			base.OnKeyDown(key);
		}

		protected override void OnMouseDown(MousePressData data)
		{
			CursorPosition = TextHelper.GetClosestPosition(
				_internalLabel.Text,
				_internalLabel.FontSize,
				data.Position.X - Padding);
			_cursorTimer = 0;

			IsFocused = true;

			base.OnMouseDown(data);
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