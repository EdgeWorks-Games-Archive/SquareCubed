using System;
using SquareCubed.Network;
using Xunit;

namespace SquareCubed.Tests.Network
{
	public class PacketHandlersTests
	{
		private readonly PacketTypes _packetTypes = new PacketTypes();
		private readonly PacketHandlers _packetHandlers = new PacketHandlers();
		private readonly PacketType _testType;

		public PacketHandlersTests()
		{
			_testType = _packetTypes.RegisterType("test.type");
		}

		[Fact]
		public void BoundTriggers()
		{
			var triggered = false;

			_packetHandlers.Bind(_testType, msg => triggered = true);
			_packetHandlers.TriggerHandler(_testType.Id, null);

			Assert.True(triggered, "Handler did not trigger!");
		}

		[Fact]
		public void UnboundThrows()
		{
			var triggered = false;

			_packetHandlers.Bind(_testType, msg => triggered = true);
			_packetHandlers.Unbind(_testType);

			Assert.Throws<InvalidOperationException>(() => _packetHandlers.TriggerHandler(_testType.Id, null));
			Assert.False(triggered, "Handler triggered!");
		}
	}
}