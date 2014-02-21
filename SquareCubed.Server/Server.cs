using System;
using System.Threading;
using SquareCubed.PluginLoader;
using SquareCubed.Utils.Logging;

namespace SquareCubed.Server
{
	public class Server : IDisposable
	{
		private readonly Logger _logger = new Logger("Server");

		#region Engine Modules

		public Network.Network Network { get; private set; }
		public PluginLoader<IServerPlugin, Server> PluginLoader { get; private set; }
		
		#region MetaData

		private readonly bool _disposeNetwork;
		private readonly bool _disposePluginLoader;

		#endregion

		#region Private Modules

		private Meta _meta;

		#endregion

		#endregion

		#region Initialization and Cleanup

		private bool _disposed;

		public Server(Network.Network network = null, bool disposeNetwork = true,
			PluginLoader<IServerPlugin, Server> pluginLoader = null, bool disposePluginLoader = true)
		{
			// Log the start of Initialization
			_logger.LogInfo("Initializing server...");

			// Yada yada
			Network = network ?? new Network.Network("SquareCubed");
			_disposeNetwork = disposeNetwork;

			// And the Plugin Loader
			PluginLoader = pluginLoader ?? new PluginLoader<IServerPlugin, Server>();
			_disposePluginLoader = disposePluginLoader;

			// Initialize the Meta Manager
			_meta = new Meta(this);

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");
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

		#region Game Loop

		#region Game Loop Events

		public event EventHandler<float> UpdateTick;

		#endregion

		public bool KeepRunning { get; set; }

		public void Run()
		{
			_logger.LogInfo("Preparing to run...");

			// Detect all installed plugins and start them
			// The server uses all plugins available, the client activates them on the fly.
			PluginLoader.DetectPlugins();
			PluginLoader.LoadAllPlugins(this);

			// Start up Network
			Network.StartServer();

			_logger.LogInfo("Started running...");
			KeepRunning = true;
			while (KeepRunning)
			{
				// Handle all queued up packets
				Network.HandlePackets();

				// Run the update event
				var updateTick = UpdateTick;
				if (updateTick != null) updateTick(this, 0.050f);

				// Fixed interval for now
				Thread.Sleep(50);
			}
			_logger.LogInfo("Finished running!");
		}

		#endregion
	}
}