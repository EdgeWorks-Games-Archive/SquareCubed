using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Gui.Controls;
using SquareCubed.Client.Window;

namespace SquareCubed.Client.Gui
{
	public class Gui : GuiControl.GuiParentControl
	{
		private Size _size;

		internal Gui(IExtGameWindow gameWindow)
		{
			_size = gameWindow.ClientSize;
		}

		public override Size Size
		{
			get { return _size; }
			set { _size = value; }
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