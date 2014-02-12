using System;
using System.Threading;
using SquareCubed.PluginLoader;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Server
{
	public class Server : IDisposable
	{
		private readonly Logger _logger;

		#region Engine Modules

		public Network.Network Network { get; private set; }
		public PluginLoader<IServerPlugin> PluginLoader { get; private set; }

		#region MetaData

		private readonly bool _disposeNetwork;
		private readonly bool _disposePluginLoader;

		#endregion

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		public Server(Network.Network network = null, bool disposeNetwork = true,
			PluginLoader<IServerPlugin> pluginLoader = null, bool disposePluginLoader = true)
		{
			// Create a Logger and log the start of Initialization
			_logger = new Logger("Server");
			_logger.LogInfo("Initializing server...");

			// Yada yada
			Network = network ?? new Network.Network();
			_disposeNetwork = disposeNetwork;

			// And the Plugin Loader
			PluginLoader = pluginLoader ?? new PluginLoader<IServerPlugin>();
			_disposePluginLoader = disposePluginLoader;

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");

			// And detect the installed plugins
			PluginLoader.DetectPlugins();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Prevent double disposing and don't dispose if we're told not to
			if (_disposed || !disposing) return;
			_disposed = true;

			// Actually dispose modules
			if (_disposeNetwork) Network.Dispose();
			if (_disposePluginLoader) PluginLoader.Dispose();
		}

		#endregion

		public void Run()
		{
			_logger.LogInfo("Started running...");
			while (true)
			{
				Thread.Sleep(200);
			}
			_logger.LogInfo("Finished running!");
		}
	}
}