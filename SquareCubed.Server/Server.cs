using System;
using System.Threading;
using SquareCubed.Common.Utils;
using SquareCubed.PluginLoader;
using SquareCubed.Server.Structures;

namespace SquareCubed.Server
{
	public class Server : IDisposable
	{
		private readonly Logger _logger = new Logger("Server");

		#region Engine Modules

		public Network.Network Network { get; private set; }
		public PluginLoader<IServerPlugin, Server> PluginLoader { get; private set; }
		public Worlds.Worlds Worlds { get; private set; }
		public Structures.Structures Structures { get; private set; }
		public Players.Players Players { get; private set; }
		public Units.Units Units { get; private set; }
		public Meta.Meta Meta { get; private set; }

		#region MetaData

		private readonly bool _disposeNetwork;
		private readonly bool _disposePluginLoader;

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

			Meta = new Meta.Meta(this);
			Worlds = new Worlds.Worlds(this);
			Structures = new Structures.Structures(this);
			Units = new Units.Units(this);
			Players = new Players.Players(this);

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

			// Start up Network
			Network.StartServer();

			// Detect all installed plugins and start them
			// The server uses all plugins available, the client activates them on the fly.
			PluginLoader.DetectPlugins();
			PluginLoader.LoadAllPlugins(this);

			_logger.LogInfo("Started running...");
			KeepRunning = true;
			const float delta = 0.05f; // < 20 Ticks per Second
			while (KeepRunning)
			{
				// Handle all queued up packets
				Network.HandlePackets();

				// Update all structures (objects and send packets)
				Structures.Update(delta);

				// Update all units (AI and send packets)
				Units.Update(delta);

				// Run the update event
				if (UpdateTick != null) UpdateTick(this, delta);

				// Fixed interval for now
				Thread.Sleep(50);
			}
			_logger.LogInfo("Finished running!");
		}

		#endregion
	}
}