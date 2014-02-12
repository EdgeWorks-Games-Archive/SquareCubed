using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;

namespace SquareCubed.Client.Graphics
{
	public class Graphics : IDisposable
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
			_window = window;
			Camera = new Camera(_window.Size);
		}

		private bool _disposed;

		public virtual void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent double disposing and don't dispose if we're told not to
			if (_disposed || !disposing) return;
			_disposed = true;

			// Nothing to do yet
		}

		#endregion

		#region Game Loop

		public virtual void BeginRender()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Initialize Camera
			Camera.SetProjectionMatrix();

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