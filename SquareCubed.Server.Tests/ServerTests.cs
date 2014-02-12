using Moq;
using SquareCubed.PluginLoader;
using Xunit;

namespace SquareCubed.Server.Tests
{
	public class ServerTestsBase
	{
		protected readonly Mock<PluginLoader<IServerPlugin>> PluginLoaderMock;
		protected readonly Mock<Network.Network> NetworkMock;

		protected ServerTestsBase()
		{
			NetworkMock = new Mock<Network.Network>();
			PluginLoaderMock = new Mock<PluginLoader<IServerPlugin>>();
		}
	}

	public class ServerTests : ServerTestsBase
	{
		[Fact]
		public void InitializesAndDisposes()
		{
			// Set Up
			Server server = null;

			Assert.DoesNotThrow(() => server = new Server(
				NetworkMock.Object, true,
				PluginLoaderMock.Object));
			Assert.DoesNotThrow(server.Dispose);
		}
	}

	public class ServerDisposeTests : ServerTestsBase
	{
		private bool _networkDisposed;
		private bool _pluginLoaderDisposed;

		public ServerDisposeTests()
		{
			NetworkMock.Setup(n => n.Dispose()).Callback(() => _networkDisposed = true);
			PluginLoaderMock.Setup(p => p.Dispose()).Callback(() => _pluginLoaderDisposed = true);
		}

		[Fact]
		public void ModulesDispose()
		{
			var server = new Server(
				NetworkMock.Object, true,
				PluginLoaderMock.Object);
			server.Dispose();

			Assert.True(_networkDisposed, "Network was not disposed.");
			Assert.True(_pluginLoaderDisposed, "Plugin Loader was not disposed.");
		}

		[Fact]
		public void ModulesDoNotDispose()
		{
			var server = new Server(
				NetworkMock.Object, false,
				PluginLoaderMock.Object, false);
			server.Dispose();

			Assert.False(_networkDisposed, "Network was disposed.");
			Assert.False(_pluginLoaderDisposed, "Plugin Loader was disposed.");
		}
	}
}