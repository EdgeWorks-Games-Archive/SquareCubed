using Moq;
using SquareCubed.PluginLoader;
using Xunit;

namespace SquareCubed.Client.Tests
{
	public class ClientTestsBase
	{
		protected readonly Mock<Graphics.Graphics> GraphicsMock;
		protected readonly Mock<PluginLoader<IClientPlugin>> PluginLoaderMock;
		protected readonly Mock<Window.Window> WindowMock;

		protected ClientTestsBase()
		{
			WindowMock = new Mock<Window.Window>();
			GraphicsMock = new Mock<Graphics.Graphics>(WindowMock.Object);
			PluginLoaderMock = new Mock<PluginLoader<IClientPlugin>>();
		}
	}

	public class ClientTests : ClientTestsBase
	{
		[Fact]
		public void InitializesAndDisposes()
		{
			Client client = null;
			Assert.DoesNotThrow(() => client = new Client(
				WindowMock.Object, true,
				GraphicsMock.Object, true,
				PluginLoaderMock.Object));
			Assert.DoesNotThrow(client.Dispose);
		}

		[Fact]
		public void CallsGraphicsInCorrectOrder()
		{
			var begin = false;
			var endAfterBegin = false;
			GraphicsMock.Setup(g => g.BeginRender()).Callback(() => begin = true);
			GraphicsMock.Setup(g => g.EndRender()).Callback(() => endAfterBegin = begin);

			var client = new Client(
				WindowMock.Object, true,
				GraphicsMock.Object, true,
				PluginLoaderMock.Object);
			client.ForceImmediateRender();

			Assert.True(begin, "Client did not call Begin Render.");
			Assert.True(endAfterBegin, "Client did not call Begin and End Render in the correct order.");
		}
	}

	public class ClientDisposeTests : ClientTestsBase
	{
		private bool _graphicsDisposed;
		private bool _pluginLoaderDisposed;
		private bool _windowDisposed;

		public ClientDisposeTests()
		{
			WindowMock.Setup(w => w.Dispose()).Callback(() => _windowDisposed = true);
			GraphicsMock.Setup(g => g.Dispose()).Callback(() => _graphicsDisposed = true);
			PluginLoaderMock.Setup(p => p.Dispose()).Callback(() => _pluginLoaderDisposed = true);
		}

		[Fact]
		public void ModulesDispose()
		{
			var client = new Client(
				WindowMock.Object, true,
				GraphicsMock.Object, true,
				PluginLoaderMock.Object);
			client.Dispose();

			Assert.True(_windowDisposed, "Window was not disposed.");
			Assert.True(_graphicsDisposed, "Graphics was not disposed.");
			Assert.True(_pluginLoaderDisposed, "Plugin Loader was not disposed.");
		}

		[Fact]
		public void ModulesDoNotDispose()
		{
			var client = new Client(
				WindowMock.Object, false,
				GraphicsMock.Object, false,
				PluginLoaderMock.Object, false);
			client.Dispose();

			Assert.False(_windowDisposed, "Window was disposed.");
			Assert.False(_graphicsDisposed, "Graphics was disposed.");
			Assert.False(_pluginLoaderDisposed, "Plugin Loader was disposed.");
		}
	}
}