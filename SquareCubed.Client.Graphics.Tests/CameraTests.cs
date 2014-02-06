using System.Drawing;
using Xunit;

namespace SquareCubed.Client.Graphics.Tests
{
	public class CameraTests
	{
		[Fact]
		public void CalculatesWidth()
		{
			var camera = new Camera(new Size(200, 100));
			camera.SetHeight(2);
			Assert.Equal(4, camera.Size.Width);
		}
	}
}
