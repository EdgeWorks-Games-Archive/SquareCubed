using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace SquareCubed.Gui
{
    public class Gui
    {
		public Size Viewport { get; set; }

	    public Gui(Size viewport)
	    {
		    Viewport = viewport;
	    }

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
