using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
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
			Units = new Units.Units(this, Structures);
			Player = new Player.UnitPlayer(this);
			Gui = new Gui.Gui(Window);

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
			Gui.Dispose();
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
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void Load(object sender, EventArgs eventArgs)
		{
#if DEBUG
			// If we're in the Debug target, add a start server button for convenience
			var startServer = new Gui.Controls.GuiButton("Start Server")
			{
				Position = new Point(Gui.Size.Width - 80, 0),
				Size = new Size(80, 21)
			};
			startServer.Click += (s, e) =>
			{
				var fileInfo = new FileInfo("../../../Server/bin/Debug/Server.exe");
				Debug.Assert(fileInfo.DirectoryName != null);
				var processInfo = new ProcessStartInfo
				{
					FileName = fileInfo.FullName,
					WorkingDirectory = fileInfo.DirectoryName
				};
				Process.Start(processInfo);
			};
			Gui.Controls.Add(startServer);
#endif

			// Now that everything is loaded, we can add the main menu
			var mainMenu = new MainMenuForm();
			mainMenu.Position = new Point(
				(Window.ClientSize.Width - mainMenu.Size.Width) / 2,
				(Window.ClientSize.Height - mainMenu.Size.Height) / 2);
			mainMenu.Connect += (s, e) =>
			{
				Network.Connect(e.HostAddress, e.PlayerName);
				ScheduledActions += () =>
				{
					Gui.Controls.Remove(mainMenu);
					mainMenu.Dispose();
#if DEBUG
					Gui.Controls.Remove(startServer);
					startServer.Dispose();
#endif
				};
			};
			mainMenu.Quit += (s, e) => Window.Close();
			Gui.Controls.Add(mainMenu);
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
		}

		public event Action ScheduledActions = () => { }; // TODO: Improve this?

		private void Update(object s, FrameEventArgs e)
		{
			ScheduledActions();
			ScheduledActions = () => { };

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

			Graphics.BeginSceneRender();

			BackgroundRenderTick(this, eventArgs);
			Structures.Render();

			Graphics.EndSceneRender();

			// Render the Gui
			Gui.Render(delta);

			Graphics.SwapBuffers();
		}

		#endregion
	}
}