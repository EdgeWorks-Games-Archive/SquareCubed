using System;
using SquareCubed.Network;
using Xunit;

namespace SquareCubed.Tests.Network
{
	public class PacketTypesTests
	{
		private readonly PacketTypes _packetTypes = new PacketTypes();

		[Fact]
		public void RegisterManual()
		{
			var type = _packetTypes.RegisterType("tests.packettype", 5);
			Assert.Equal("tests.packettype", type.Name);
			Assert.Equal(5, type.Id);
		}

		[Fact]
		public void PreventDoubleNameManual()
		{
			_packetTypes.RegisterType("tests.packettype", 5);
			Assert.Throws<InvalidOperationException>(() => _packetTypes.RegisterType("tests.packettype", 8));
		}

		[Fact]
		public void PreventDoubleNameAuto()
		{
			_packetTypes.RegisterType("tests.packettype");
			Assert.Throws<InvalidOperationException>(() => _packetTypes.RegisterType("tests.packettype"));
		}

		[Fact]
		public void PreventDoubleId()
		{
			_packetTypes.RegisterType("tests.packettype", 6);
			Assert.Throws<InvalidOperationException>(() => _packetTypes.RegisterType("tests.othertype", 6));
		}

		[Fact]
		public void RegisterAuto()
		{
			var type = _packetTypes.RegisterType("tests.packettype");
			var otherType = _packetTypes.RegisterType("tests.othertype");
			Assert.Equal("tests.packettype", type.Name);
			Assert.NotEqual(otherType.Id, type.Id);
		}

		[Fact]
		public void AutoAndManualDoNotCollide()
		{
			var type = _packetTypes.RegisterType("tests.packettype", 1);
			var anotherType = _packetTypes.RegisterType("tests.secondtype", 2);
			var autoType = _packetTypes.RegisterType("tests.othertype");
			Assert.NotEqual(type.Id, autoType.Id);
			Assert.NotEqual(anotherType.Id, autoType.Id);
		}

		[Fact]
		public void Lookup()
		{
			var expected = _packetTypes.RegisterType("stuff.sometype");
			var type = _packetTypes.ResolveType("stuff.sometype");
			Assert.Equal(expected, type);
		}

		[Fact]
		public void CannotLookupUnknown()
		{
			_packetTypes.RegisterType("tests.packettype", 5);
			Assert.Throws<InvalidOperationException>(() => _packetTypes.ResolveType("random.stuff"));
		}
	}
}