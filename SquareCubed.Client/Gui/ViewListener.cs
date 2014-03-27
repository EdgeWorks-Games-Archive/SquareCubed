using System.Diagnostics;
using Coherent.UI;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui
{
	internal class ViewListener : Coherent.UI.ViewListener
	{
		private readonly TextureBufferHelper _helper = new TextureBufferHelper();
		private readonly Texture2D _texture;
		private View _view;

		public ViewListener(Texture2D sharedTexture)
		{
			_texture = sharedTexture;
		}

		public override void OnViewCreated(View view)
		{
			_view = view;
			_view.SetFocus();

			base.OnViewCreated(view);
		}

		public override void OnDraw(CoherentHandle handle, bool usesSharedMemory, int width, int height)
		{
			Debug.Assert(usesSharedMemory, "Coherent UI must be set to use shared memory.");

			// Get the shared texture memory and map our opengl texture to it
			var pixels = _helper.MapSharedMemoryNative(handle, width, height);
			_texture.MapSubImage(pixels);

			// TODO: Release shared memory here? I cannot find if this is needed.

			base.OnDraw(handle, true, width, height);
		}

		public override void CreateSurface(bool sharedMemory, uint width, uint height, SurfaceResponse response)
		{
			Debug.Assert(sharedMemory, "Coherent UI must be set to use shared memory.");

			// Create the new shared memory and signal it to the response
			var sharedHandle = _helper.CreateSharedMemory(width, height);
			response.Signal(sharedHandle);

			// Do not invoke base
		}

		public override void DestroySurface(CoherentHandle surface, bool usesSharedMemory)
		{
			Debug.Assert(usesSharedMemory, "Coherent UI must be set to use shared memory.");

			// Destroy the shared memory
			_helper.DestroySharedMemory(surface);

			// Do not invoke base
		}
	}
}