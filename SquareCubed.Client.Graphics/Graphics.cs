using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;

namespace SquareCubed.Client.Graphics
{
	public class Graphics : IDisposable
	{
		#region External Modules

		private Window.Window _window;

		#endregion

		#region Initialization and Cleanup

		public Graphics(Window.Window window)
		{
			_window = window;
		}

		private bool _disposed;

		public virtual void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent Double Disposing
			if (_disposed) return;

			if (disposing)
			{
				// Nothing to do yet
			}

			_disposed = true;
		}

		#endregion

		#region Game Loop

		public virtual void BeginRender()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Initialize Camera
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

			// Render Test Triangle
			GL.Begin(PrimitiveType.Triangles);

			GL.Color3(Color.Red);
			GL.Vertex2(-1.0f, 1.0f); // top left
			GL.Color3(Color.Lime);
			GL.Vertex2(0.0f, -1.0f);
			GL.Color3(Color.Blue);
			GL.Vertex2(1.0f, 1.0f);

			GL.End();
		}

		public virtual void EndRender()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}