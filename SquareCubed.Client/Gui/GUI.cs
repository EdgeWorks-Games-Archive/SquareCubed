using System;
using System.Diagnostics.Contracts;
using Coherent.UI;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Client.Graphics;
using SquareCubed.Client.Graphics.Shaders;

namespace SquareCubed.Client.Gui
{
	public sealed class Gui : IDisposable
	{
		private readonly Client _client;

		#region Coherent UI Resources

		private EventListener _eventListener;
		private ViewListener _viewListener;
		private SystemSettings _settings;
		private UISystem _system;

		#endregion

		#region Graphics Resources

		private ShaderProgram _program;
		private ProgramUniform _textureSampler;
		private Texture2D _texture;
		private VertexBuffer _vertexBuffer;

		#endregion

		public bool IsLoaded { get; private set; }

		public Gui(Client client)
		{
			_client = client;
		}

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
			_texture = new Texture2D(_client.Window.Width, _client.Window.Height, true, false, true);

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

				// Create a new test view
				var viewInfo = new ViewInfo
				{
					Width = _client.Window.Width,
					Height = _client.Window.Height,
					IsTransparent = true,
					UsesSharedMemory = true
				};
				_system.CreateView(viewInfo, "coui://GUI/Test.html", _viewListener);
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
						5 * sizeof(float), // Offset between values
						3 * sizeof(float)); // Start offset

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