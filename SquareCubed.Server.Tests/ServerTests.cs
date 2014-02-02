using Xunit;

namespace SquareCubed.Server.Tests
{
	public class ServerTests
    {
		[Fact]
		public void InitializesWithoutExceptions()
		{
			Assert.DoesNotThrow(() => new Server());
		}
    }
}