using System;
using System.Diagnostics;
using System.IO;
using OpenTK;
using SquareCubed.Client.Window;
using SquareCubed.Common.Utils;
using SquareCubed.PluginLoader;

namespace SquareCubed.Client
{
	public sealed class Client : IDisposable
	{
		private readonly Logger _logger = new Logger("Client");

		#region Engine Modules

		public Graphics.Graphics Graphics { get; private set; }
		public Network.Network Network { get; private set; }
		public PluginLoader<IClientPlugin, Client> PluginLoader { get; private set; }
		public IExtGameWindow Window { get; private set; }
		public Input.Input Input { get; private set; }
		public Gui.Gui Gui { get; private set; }
		public Player.IPlayer Player { get; private set; }
		public Meta.Meta Meta { get; private set; }
		public Units.Units Units { get; private set; }
		public Structures.Structures Structures { get; private set; }

		#endregion

		#region Events

		public event EventHandler<TickEventArgs> UpdateTick = (o, p) => { };
		public event EventHandler<TickEventArgs> BackgroundRenderTick = (o, p) => { };

		#endregion

		#region Initialization and Cleanup

		/// <summary>
		///     Initializes a new instance of the <see cref="Client" /> class.
		/// </summary>
		public Client()
		{
			// Log the start of Initialization
			_logger.LogInfo("Initializing client...");

			// Initialize all the submodules
			Window = new Window.Window();
			Graphics = new Graphics.Graphics(Window);
			Input = new Input.Input(Window, Graphics.Camera);
			Network = new Network.Network("SquareCubed");
			PluginLoader = new PluginLoader<IClientPlugin, Client>();
			Meta = new Meta.Meta(this);
			Structures = new Structures.Structures(this);
			Units = new Units.Units(this);
			Player = new Player.UnitPlayer(this);

			// Hook Game Loop Events
			Window.Load += Load;
			Window.Unload += Unload;
			Window.UpdateFrame += Update;
			Window.RenderFrame += Render;

			// Done initializing, let's log it
			_logger.LogInfo("Finished initializing engine!");
		}

		public void Dispose()
		{
			// We only have managed resources to dispose of
			PluginLoader.Dispose();
			Network.Dispose();
			Window.Dispose();
		}

		#endregion

		#region Game Loop

		/// <summary>
		///     Runs this instance.
		/// </summary>
		public void Run()
		{
			_logger.LogInfo("Preparing to run...");

			// Detect all installed plugins
			PluginLoader.DetectPlugins();

			_logger.LogInfo("Started running...");
			Window.Run();
			_logger.LogInfo("Finished running!");
		}

		/// <summary>
		///     Load is called after the context and opengl is set up.
		///     Stuff that needs to be initialized after opengl is set
		///     up should be called in here.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void Load(object s, EventArgs e)
		{
			Gui = new Gui.Gui(this);
			
			// Bind some default functions
			Gui.BindCall("quit", Window.Close);
			Gui.BindCall<string, string>("connect", (host, name) => Network.Connect(host, name));
			Gui.BindCall("disconnect", Network.Disconnect);
#if DEBUG
			Gui.BindCall("server.start", () =>
			{
				var fileInfo = new FileInfo("../../../Server/bin/Debug/Server.exe");
				Debug.Assert(fileInfo.DirectoryName != null);
				var processInfo = new ProcessStartInfo()
				{
					FileName = fileInfo.FullName,
					WorkingDirectory = fileInfo.DirectoryName
				};
				Process.Start(processInfo);
			});
#endif

			// Add some event triggers
			// TODO: Make LostConnection only trigger when a connection was lost, not failed
			Network.LostConnection += (se, ev) => Gui.Trigger("Network.ConnectFailed");

			// Make the main menu hide once we connected
			Network.NewConnection += (se, ev) => Gui.MainMenu.Hide();
		}

		/// <summary>
		///     Unload is called after the engine is done running, but
		///     before cleaning up the context and opengl. Clean up any
		///     OpenGL related objects here.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void Unload(object s, EventArgs e)
		{
			PluginLoader.UnloadAllPlugins();

			Gui.Dispose();
			Gui = null;
		}

		private void Update(object s, FrameEventArgs e)
		{
			// Gui needs to be updated as early as possible
			Gui.Update();

			// Clamp tick data to prevent long frame stutters from messing stuff up
			var delta = e.Time > 0.1f ? 0.1f : (float) e.Time;
			var eventArgs = new TickEventArgs {ElapsedTime = delta};

			// Handle all queued up packets
			Network.HandlePackets();

			// Update the axises before updating
			Input.UpdateAxes();

			// Run the update event
			UpdateTick(this, eventArgs);
		}

		private void Render(object s, FrameEventArgs e)
		{
			// Clamp tick data to prevent long frame stutters from messing stuff up
			var delta = e.Time > 0.1f ? 0.1f : (float) e.Time;
			var eventArgs = new TickEventArgs {ElapsedTime = delta};

			Graphics.BeginRender();

			// Run the background render event
			BackgroundRenderTick(this, eventArgs);

			Structures.Render();

			Graphics.EndRender();

			// Render the Gui
			Graphics.BeginRenderGui();
			Gui.Render();

			Graphics.EndRenderAll();
		}

		#endregion
	}
}