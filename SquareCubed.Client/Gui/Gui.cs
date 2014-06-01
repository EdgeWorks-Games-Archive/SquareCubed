using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Window;

namespace SquareCubed.Client.Gui
{
	public class Gui
	{
		internal Gui(IExtGameWindow gameWindow)
		{
			Viewport = gameWindow.ClientSize;
		}

		public Size Viewport { get; set; }

		public void Render()
		{
			// Set framebuffer to the default one
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Viewport(0, 0, Viewport.Width, Viewport.Height);

			// Reset the matrices to default values
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}
	}
}