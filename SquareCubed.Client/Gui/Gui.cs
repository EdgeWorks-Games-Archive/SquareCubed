using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SquareCubed.Client.Gui.Controls;
using SquareCubed.Client.Window;

namespace SquareCubed.Client.Gui
{
	public class Gui : GuiControl.GuiParentControl
	{
		private Size _size;
		private Point _previousMousePos;

		internal Gui(IExtGameWindow gameWindow)
		{
			_size = gameWindow.ClientSize;
			gameWindow.MouseMove += OnMouseMoveEvent;
		}

		public override Size Size
		{
			get { return _size; }
			set { _size = value; }
		}

		private void OnMouseMoveEvent(object sender, MouseMoveEventArgs e)
		{
			var moveData = new MouseMoveData(e.Position, _previousMousePos);

			HandleMouseMove(moveData);

			_previousMousePos = e.Position;
		}

		public override Size InternalOffset
		{
			get { return new Size(0, 0); }
		}

		public override void Render()
		{
			// Set framebuffer to the default one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Viewport(0, 0, _size.Width, _size.Height);

			// Reset the matrices to default values
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, _size.Width, _size.Height, 0, 1, -1);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			base.Render();
		}
	}
}