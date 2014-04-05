using System;
using System.Threading;
using SquareCubed.Common.Utils;
using SquareCubed.PluginLoader;

namespace SquareCubed.Server
{
	public sealed class Server : IDisposable
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

		#endregion

		#region Events

		public event EventHandler<float> UpdateTick = (o, p) => { };

		#endregion

		#region Initialization and Cleanup

		public Server()
		{
			// Log the start of Initialization
			_logger.LogInfo("Initializing server...");

			Network = new Network.Network("SquareCubed");
			PluginLoader = new PluginLoader<IServerPlugin, Server>();
			Meta = new Meta.Meta(this);
			Worlds = new Worlds.Worlds(this);
			Structures = new Structures.Structures(Network);
			Units = new Units.Units(Network);
			Players = new Players.Players(this);

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");
		}

		public void Dispose()
		{
			// We only have managed resources to dispose of
			PluginLoader.Dispose();
			Network.Dispose();
		}

		#endregion

		#region Game Loop

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
				UpdateTick(this, delta);

				// Fixed interval for now
				Thread.Sleep(50);
			}
			_logger.LogInfo("Finished running!");
		}

		#endregion
	}
}