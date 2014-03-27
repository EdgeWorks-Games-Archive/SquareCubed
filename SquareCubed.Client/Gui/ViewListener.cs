using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Coherent.UI;
using OpenTK.Graphics.ES10;
using SquareCubed.Client.Graphics;

namespace SquareCubed.Client.Gui
{
	internal class ViewListener : Coherent.UI.ViewListener // TODO: Destroy view in Dispose
	{
		private View _view;
		private readonly TextureBufferHelper _helper = new TextureBufferHelper();
		private readonly Texture2D _texture;

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

			// Get the shared texture memory
			var pixels = _helper.MapSharedMemoryNative(handle, width, height);

			// Map the texture memory we received to our texture
			_texture.MapSubImage(pixels);

			// Release the shared texture memory
			// TODO: Release here? I cannot find if this is needed.

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
