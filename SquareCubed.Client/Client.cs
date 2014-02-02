using System;

namespace SquareCubed.Client
{
	public class Client : IDisposable
	{
		public Window.Window Window { get; private set; }

		private readonly bool _disposeWindow;

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		/// <param name="window">If not null, use this existing window.</param>
		/// <param name="disposeWindow">If false, doesn't dispose the window.</param>
		public Client(Window.Window window = null, bool disposeWindow = true)
		{
			// If caller doens't provide a window, create our own
			Window = window ?? new Window.Window();
			_disposeWindow = disposeWindow;
		}

		public void Run()
		{
			
		}

		public void Dispose()
		{
			if(_disposeWindow) Window.Dispose();
		}
	}
}