using Xunit;

namespace SquareCubed.Server.Tests
{
	public class ServerTests
	{
		[Fact]
		public void InitializesAndDisposes()
		{
			// Set Up
			Server server = null;

			Assert.DoesNotThrow(() => server = new Server());
			Assert.DoesNotThrow(server.Dispose);
		}
	}
}