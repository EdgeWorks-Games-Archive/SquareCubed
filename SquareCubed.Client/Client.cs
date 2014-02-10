using System;
using OpenTK;
using SquareCubed.PluginLoader;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Client
{
	public class Client : IDisposable
	{
		private readonly Logger _logger;

		#region Engine Modules

		public Graphics.Graphics Graphics { get; private set; }
		public PluginLoader<IClientPlugin> PluginLoader { get; private set; }
		public Window.Window Window { get; private set; }

		#region MetaData

		private readonly bool _disposeGraphics;
		private readonly bool _disposePluginLoader;
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
		/// <param name="pluginLoader">If not null, use this existing plugin loader module.</param>
		/// <param name="disposePluginLoader">If false, doesn't dispose the plugin loader module.</param>
		public Client(Window.Window window = null, bool disposeWindow = true,
			Graphics.Graphics graphics = null, bool disposeGraphics = true,
			PluginLoader<IClientPlugin> pluginLoader = null, bool disposePluginLoader = true)
		{
			// Create a Logger and Log the start of Initialization
			_logger = new Logger("Client");
			_logger.LogInfo("Initializing engine...");

			// If caller doesn't provide a window, create our own
			Window = window ?? new Window.Window();
			_disposeWindow = disposeWindow;

			// Same for graphics
			Graphics = graphics ?? new Graphics.Graphics(Window);
			_disposeGraphics = disposeGraphics;

			// And the Plugin Loader
			PluginLoader = pluginLoader ?? new PluginLoader<IClientPlugin>();
			_disposePluginLoader = disposePluginLoader;

			// Hook Game Loop Events
			Window.UpdateFrame += (s, e) => Update(e);
			Window.RenderFrame += (s, e) => Render(e);

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");

			// And detect the installed plugins
			PluginLoader.DetectPlugins();
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
				if (_disposePluginLoader) PluginLoader.Dispose();
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
			_logger.LogInfo("Started running...");
			Window.Run();
			_logger.LogInfo("Finished running!");
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