using System.Drawing;
using SquareCubed.Client.Graphics;
using Xunit;

namespace SquareCubed.Tests.Client.Graphics
{
	public class CameraTests
	{
		private readonly Camera _camera;

		public CameraTests()
		{
			// Easy to work with values
			var size = new Size(150, 100);
			_camera = new Camera(size);
		}

		[Fact]
		public void ResolutionSetsCorrectRatio()
		{
			Assert.Equal(1.5f, _camera.AspectRatio);
		}

		[Fact]
		public void SetHeightMaintainsRatio()
		{
			_camera.Height = 20.2f;
			Assert.Equal(30.3f, _camera.Width, 5);
		}

		[Fact]
		public void SizePropertyEqualsWidthAndHeight()
		{
			Assert.Equal(_camera.Width, _camera.Size.Width);
			Assert.Equal(_camera.Height, _camera.Size.Height);
		}
	}
}