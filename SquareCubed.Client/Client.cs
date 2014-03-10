using System;
using OpenTK;
using SquareCubed.Common.Utils;
using SquareCubed.PluginLoader;

namespace SquareCubed.Client
{
	public class Client : IDisposable
	{
		private readonly Logger _logger = new Logger("Client");

		#region Engine Modules

		public Graphics.Graphics Graphics { get; private set; }
		public Network.Network Network { get; private set; }
		public PluginLoader<IClientPlugin, Client> PluginLoader { get; private set; }
		public Window.Window Window { get; private set; }
		public Input.Input Input { get; private set; }
		public Player.Player Player { get; private set; }
		public Meta.Meta Meta { get; private set; }
		public Units.Units Units { get; private set; }
		public Structures.Structures Structures { get; private set; }

		#region MetaData

		private readonly bool _disposeGraphics;
		private readonly bool _disposeNetwork;
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
		/// <param name="network">If not null, use this existing network module.</param>
		/// <param name="disposeNetwork">If false, doesn't dispose the network module.</param>
		/// <param name="pluginLoader">If not null, use this existing plugin loader module.</param>
		/// <param name="disposePluginLoader">If false, doesn't dispose the plugin loader module.</param>
		public Client(Window.Window window = null, bool disposeWindow = true,
			Graphics.Graphics graphics = null, bool disposeGraphics = true,
			Network.Network network = null, bool disposeNetwork = true,
			PluginLoader<IClientPlugin, Client> pluginLoader = null, bool disposePluginLoader = true)
		{
			// Log the start of Initialization
			_logger.LogInfo("Initializing client...");

			// If caller doesn't provide a window, create our own
			Window = window ?? new Window.Window();
			_disposeWindow = disposeWindow;

			// Same for graphics
			Graphics = graphics ?? new Graphics.Graphics(Window);
			_disposeGraphics = disposeGraphics;

			// The Input
			Input = new Input.Input(Window);

			// The Network
			Network = network ?? new Network.Network("SquareCubed");
			_disposeNetwork = disposeNetwork;

			// And the Plugin Loader
			PluginLoader = pluginLoader ?? new PluginLoader<IClientPlugin, Client>();
			_disposePluginLoader = disposePluginLoader;

			Meta = new Meta.Meta(this);
			Structures = new Structures.Structures(this);
			Units = new Units.Units(this);
			Player = new Player.Player(this);

			// Hook Game Loop Events
			Window.UpdateFrame += (s, e) => Update(e);
			Window.RenderFrame += (s, e) => Render(e);

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");
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
				if (_disposeNetwork) Network.Dispose();
				if (_disposePluginLoader) PluginLoader.Dispose();
			}

			_disposed = true;
		}

		#endregion

		#region Game Loop

		#region Game Loop Events

		public event EventHandler<float> UpdateTick;
		public event EventHandler<float> BackgroundRenderTick;
		public event EventHandler<float> UnitRenderTick;

		#endregion

		/// <summary>
		///     Runs this instance.
		/// </summary>
		public void Run()
		{
			_logger.LogInfo("Preparing to run...");

			// Detect all installed plugins
			PluginLoader.DetectPlugins();

			// TODO: Move to connect input form
			Network.Connect("127.0.0.1");

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
			// Handle all queued up packets
			Network.HandlePackets();

			// Update the axises before updating
			Input.UpdateAxes();

			Player.Update((float) e.Time);

			// Run the update event
			if (UpdateTick != null) UpdateTick(this, (float) e.Time);
		}

		private void Render(FrameEventArgs e)
		{
			Graphics.BeginRender();

			// Run the background render event
			if (BackgroundRenderTick != null) BackgroundRenderTick(this, (float) e.Time);

			Structures.Render();

			// Run the unit render event
			if (UnitRenderTick != null) UnitRenderTick(this, (float) e.Time);

			// Render some test stuff in player
			Player.Render();

			Graphics.EndRender();
		}

		#endregion
	}
}