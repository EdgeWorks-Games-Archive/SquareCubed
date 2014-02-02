using System;

namespace SquareCubed.Client
{
	public class Client : IDisposable
	{
		#region Engine Modules

		public Window.Window Window { get; private set; }

		#region MetaData

		private readonly bool _disposeWindow;

		#endregion

		#endregion

		#region Initialization and Cleanup

		/// <summary>
		///     Initializes a new instance of the <see cref="Client" /> class.
		/// </summary>
		/// <param name="window">If not null, use this existing window.</param>
		/// <param name="disposeWindow">If false, doesn't dispose the window.</param>
		public Client(Window.Window window = null, bool disposeWindow = true)
		{
			// If caller doens't provide a window, create our own
			Window = window ?? new Window.Window();
			_disposeWindow = disposeWindow;
		}

		public void Dispose()
		{
			if (_disposeWindow) Window.Dispose();
		}

		#endregion

		public void Run()
		{
		}
	}
}