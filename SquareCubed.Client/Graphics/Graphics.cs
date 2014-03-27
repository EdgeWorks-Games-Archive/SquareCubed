using System.Diagnostics.Contracts;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;

namespace SquareCubed.Client.Graphics
{
	public class Graphics
	{
		#region External Modules

		private readonly Window.Window _window;

		#endregion

		#region Camera

		public Camera Camera { get; private set; }

		#endregion

		#region Initialization and Cleanup

		public Graphics(Window.Window window)
		{
			Contract.Requires<ArgumentNullException>(window != null);

			_window = window;
			Camera = new Camera(_window.Size);
		}

		#endregion

		#region Game Loop

		public virtual void BeginRender()
		{
			// Ensure settings are set correctly
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Multisample);

			GL.ClearColor(Color.FromArgb(5, 5, 8));
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Initialize Camera
			Camera.SetMatrices();
		}

		public virtual void EndRender()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}