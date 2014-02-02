using System;
using OpenTK;

namespace SquareCubed.Client
{
	public class Client : IDisposable
	{
		#region Engine Modules

		public Window.Window Window { get; private set; }
		public Graphics.Graphics Graphics { get; private set; }

		#region MetaData

		private readonly bool _disposeGraphics;
		private readonly bool _disposeWindow;

		#endregion

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		/// <summary>
		///     Initializes a new instance of the <see cref="Client" /> class.
		/// </summary>
		/// <param name="window">If not null, use this existing window.</param>
		/// <param name="disposeWindow">If false, doesn't dispose the window.</param>
		/// <param name="graphics">If not null, use this existing graphics module.</param>
		/// <param name="disposeGraphics">If false, doesn't dispose the graphics module.</param>
		public Client(Window.Window window = null, bool disposeWindow = true,
			Graphics.Graphics graphics = null, bool disposeGraphics = true)
		{
			// If caller doesn't provide a window, create our own
			Window = window ?? new Window.Window();
			_disposeWindow = disposeWindow;

			// Same for graphics
			Graphics = graphics ?? new Graphics.Graphics(Window);
			_disposeGraphics = disposeGraphics;

			// Hook Game Loop Events
			Window.UpdateFrame += (s, e) => Update(e);
			Window.RenderFrame += (s, e) => Render(e);
		}

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
				if (_disposeWindow) Window.Dispose();
				if (_disposeGraphics) Graphics.Dispose();
			}

			_disposed = true;
		}

		#endregion

		#region Game Loop

		/// <summary>
		///     Runs this instance.
		/// </summary>
		public void Run()
		{
			Window.Run();
		}

		public void ForceImmediateRender()
		{
			Render(new FrameEventArgs());
		}

		private void Update(FrameEventArgs e)
		{
		}

		private void Render(FrameEventArgs e)
		{
			Graphics.BeginRender();
			Graphics.EndRender();
		}

		#endregion
	}
}