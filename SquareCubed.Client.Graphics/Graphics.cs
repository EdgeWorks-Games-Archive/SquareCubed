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
			// Ensure settings are set correctly
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			// Initialize Camera
			Camera.SetProjectionMatrix();
		}

		public virtual void EndRender()
		{
			_window.SwapBuffers();
		}

		#endregion
	}
}