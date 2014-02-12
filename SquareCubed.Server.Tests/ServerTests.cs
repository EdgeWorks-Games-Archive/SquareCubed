using Moq;
using SquareCubed.PluginLoader;
using Xunit;

namespace SquareCubed.Server.Tests
{
	public class ServerTestsBase
	{
		protected readonly Mock<PluginLoader<IServerPlugin>> PluginLoaderMock;

		protected ServerTestsBase()
		{
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
				PluginLoaderMock.Object));
			Assert.DoesNotThrow(server.Dispose);
		}
	}

	public class ServerDisposeTests : ServerTestsBase
	{
		private bool _pluginLoaderDisposed;

		public ServerDisposeTests()
		{
			PluginLoaderMock.Setup(p => p.Dispose()).Callback(() => _pluginLoaderDisposed = true);
		}

		[Fact]
		public void ModulesDispose()
		{
			var server = new Server(
				PluginLoaderMock.Object);
			server.Dispose();

			Assert.True(_pluginLoaderDisposed, "Plugin Loader was not disposed.");
		}

		[Fact]
		public void ModulesDoNotDispose()
		{
			var server = new Server(
				PluginLoaderMock.Object, false);
			server.Dispose();

			Assert.False(_pluginLoaderDisposed, "Plugin Loader was disposed.");
		}
	}
}