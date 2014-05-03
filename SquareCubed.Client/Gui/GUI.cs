using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Coherent.UI;
using Coherent.UI.Binding;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;
using SquareCubed.Client.Graphics.Shaders;
using SquareCubed.Client.Gui.Panels;

namespace SquareCubed.Client.Gui
{
	public sealed class Gui : IDisposable
	{
		private readonly Client _client;

		#region Coherent UI Resources

		private EventListener _eventListener;
		private SystemSettings _settings;
		private UISystem _system;
		private ViewListener _viewListener;

		#endregion

		#region Graphics Resources

		private ShaderProgram _program;
		private Texture2D _texture;
		private ProgramUniform _textureSampler;
		private VertexBuffer _vertexBuffer;

		#endregion

		#region Panels

		public MainMenuPanel MainMenu { get; private set; }
		public EscMenuPanel EscMenu { get; private set; }

		#endregion

		#region View Interaction

		private readonly List<Action> _viewReadyQueue = new List<Action>();

		public void Trigger(string func)
		{
			// TODO: This isn't a very elegant way to do it but it works for now.
			if (_viewListener != null && _viewListener.View != null)
				_viewListener.View.TriggerEvent(func);
			else
				_viewReadyQueue.Add(() => _viewListener.View.TriggerEvent(func));
		}

		public void Trigger<T>(string func, T param)
		{
			// TODO: This isn't a very elegant way to do it but it works for now.
			if (_viewListener != null && _viewListener.View != null)
				_viewListener.View.TriggerEvent(func, param);
			else
				_viewReadyQueue.Add(() => _viewListener.View.TriggerEvent(func, param));
		}

		public void Trigger<T1, T2>(string func, T1 param1, T2 param2)
		{
			// TODO: This isn't a very elegant way to do it but it works for now.
			if (_viewListener != null && _viewListener.View != null)
				_viewListener.View.TriggerEvent(func, param1, param2);
			else
				_viewReadyQueue.Add(() => _viewListener.View.TriggerEvent(func, param1, param2));
		}

		public void AddPanel(string html)
		{
			Trigger("AddPanel", html);
		}

		public void RemovePanel(string pattern)
		{
			Trigger("RemovePanel", pattern);
		}

		public void AddScript(string src)
		{
			Trigger("AddScript", src);
		}

		public void AddStyle(string src)
		{
			Trigger("AddStyle", src);
		}

		public void BindCall(string name, Action handler)
		{
			// TODO: This isn't a very elegant way to do it but it works for now.
			if (_viewListener != null && _viewListener.View != null)
				_viewListener.View.BindCall(name, handler);
			else
				_viewReadyQueue.Add(() => _viewListener.View.BindCall(name, handler));
		}

		public void BindCall<T>(string name, Action<T> handler)
		{
			// TODO: This isn't a very elegant way to do it but it works for now.
			if (_viewListener != null && _viewListener.View != null)
				_viewListener.View.BindCall(name, handler);
			else
				_viewReadyQueue.Add(() => _viewListener.View.BindCall(name, handler));
		}

		#endregion

		private readonly InputHandler _inputHandler;

		public Gui(Client client)
		{
			Contract.Requires<ArgumentNullException>(client != null);

			_client = client;
			_inputHandler = new InputHandler(_client.Window);
		}

		public bool IsLoaded { get; private set; }

		public void Dispose()
		{
			// We only have managed resources to dispose of
			if (IsLoaded) Unload();
			if (_viewListener != null) _viewListener.Dispose();
		}

		public void Load()
		{
			Contract.Requires<InvalidOperationException>(
				!IsLoaded,
				"GUI is already loaded.");

			// Set up Event Listener
			_eventListener = new EventListener();

			// Configure CoherentUI
			_settings = new SystemSettings
			{
#if DEBUG
				DebuggerPort = 1234
#endif
			};

			// Set up the UI System
			_system = CoherentUI_Native.InitializeUISystem(
				Versioning.SDKVersion, License.COHERENT_KEY,
				_settings, _eventListener,
				Severity.Warning, null, null);

			// Make sure it got created
			if (_system == null)
				throw new Exception("Failed to initialize CoherentUI!");

			// Create a shader program for Coherent UI
			_program = new ShaderProgram(
				"Shaders/CoherentUI.vert",
				"Shaders/CoherentUI.frag");

			// Create a texture for it as well
			// The texture needs to have alpha activated since Coherent UI will need it
			_texture = new Texture2D(_client.Window.Width, _client.Window.Height, TextureOptions.Alpha | TextureOptions.Bgra);

			// Get the texture sampler uniform
			_textureSampler = _program.GetUniform("textureSampler");

			float[] vertexData =
			{
				// -- position --     -- uv --
				-1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
				3.0f, -1.0f, 0.0f, 2.0f, 1.0f,
				-1.0f, 3.0f, 0.0f, 0.0f, -1.0f
			};

			// Create a new vertex buffer with the vertex data we need
			_vertexBuffer = new VertexBuffer(vertexData);

			// Create Panels
			MainMenu = new MainMenuPanel(this);

			EscMenu = new EscMenuPanel(this);

			IsLoaded = true;
		}

		public void Unload()
		{
			Contract.Requires<InvalidOperationException>(
				IsLoaded,
				"GUI needs to be loaded before it can be unloaded.");

			// Clean up the Coherent UI system
			_system.Uninitialize();
			_system.Dispose();
			_system = null;
			_settings = null;

			// Clean up all the Graphics Resources
			_program.Dispose();
			_program = null;

			// Clean up the Listeners
			_eventListener.Dispose();
			_eventListener = null;

			// Clean up the Graphics Resources
			// TODO: _texture.Dispose() needs to be created
			_texture = null;
			// TODO: _vertexBuffer.Dispose() needs to be created
			_vertexBuffer = null;

			IsLoaded = false;
		}

		public void Update()
		{
			// Update Coherent UI
			_system.Update();
		}

		public void Render()
		{
			// If we can create the view listener and it's not yet created
			if (_eventListener.IsSystemReady && _viewListener == null)
			{
				// Create the view listener
				_viewListener = new ViewListener(_texture);
				_viewListener.FinishLoad += (frameId, validatedPath, isMainFrame, statusCode, headers) =>
				{
					_viewReadyQueue.ForEach(a => a.Invoke());
					_viewReadyQueue.Clear();
				};

				// Create a new test view
				var viewInfo = new ViewInfo
				{
					Width = _client.Window.Width,
					Height = _client.Window.Height,
					IsTransparent = true,
					UsesSharedMemory = true
				};
				_system.CreateView(viewInfo, "coui://GUI/Base.html", _viewListener);

				// Pass over the view listener to the input handler as well
				_inputHandler.ViewListener = _viewListener;
			}

			// Get the latest Coherent UI surfaces
			_system.FetchSurfaces();

			// Reset the matrices to default values
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();

			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();

			using (_program.Activate())
			using (_texture.Activate())
			{
				// Set the texture sampler to use texture unit 0
				_textureSampler.SetInt32(0);

				// Enable the attribute arrays so we can send attributes
				// TODO: Gotta make this part of the vertex buffer object
				GL.EnableVertexAttribArray(0);
				GL.EnableVertexAttribArray(1);
				using (_vertexBuffer.Activate())
				{
					// Set the position attribute pointer
					GL.VertexAttribPointer(
						0, // Location
						3, // Size
						VertexAttribPointerType.Float, // Type
						false, // Normalized
						5*sizeof (float), // Offset between values
						0); // Start offset

					// Set the UV attribute pointer
					GL.VertexAttribPointer(
						1, // Location
						2, // Size
						VertexAttribPointerType.Float, // Type
						false, // Normalized
						5*sizeof (float), // Offset between values
						3*sizeof (float)); // Start offset

					// And finally, draw
					GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
				}

				// Clean up
				GL.DisableVertexAttribArray(0);
				GL.DisableVertexAttribArray(1);
			}

			// Reset the matrices
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();

			GL.MatrixMode(MatrixMode.Modelview);
			GL.PopMatrix();
		}
	}
}