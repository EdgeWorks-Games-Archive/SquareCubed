using Moq;
using Xunit;

namespace SquareCubed.Client.Tests
{
	public class ClientTests
	{
		[Fact]
		public void InitializesAndDisposes()
		{
			// Set Up
			var window = new Mock<Window.Window>();
			Client client = null;

			Assert.DoesNotThrow(() => client = new Client(window.Object));
			Assert.DoesNotThrow(client.Dispose);
		}
	}

	public class ClientDisposeTests
	{
		private readonly Mock<Window.Window> _windowMock = new Mock<Window.Window>();
		private bool _disposed;

		public ClientDisposeTests()
		{
			_windowMock.Setup(w => w.Dispose()).Callback(() => _disposed = true);
		}

		[Fact]
		public void GameWindowDisposes()
		{
			var client = new Client(_windowMock.Object);
			client.Dispose();
			Assert.True(_disposed, "GameWindow was not disposed.");
		}

		[Fact]
		public void GameWindowDoesntDispose()
		{
			var client = new Client(_windowMock.Object, false);
			client.Dispose();
			Assert.False(_disposed, "GameWindow was disposed.");
		}
	}
}