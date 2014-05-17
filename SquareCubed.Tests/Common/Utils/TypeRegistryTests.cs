using System;
using Moq;
using SquareCubed.Common.Utils;
using Xunit;

namespace SquareCubed.Tests.Common.Utils
{
	public class TypeRegistryTests
	{
		private readonly TypeRegistry<ITestType> _registry = new TypeRegistry<ITestType>();

		[Fact]
		public void LookupById()
		{
			_registry.RegisterType(Mock.Of<ITestType>(t => t.TestValue == 5), 8);
			_registry.RegisterType(Mock.Of<ITestType>(t => t.TestValue == 16), 3);

			Assert.Equal(5, _registry.GetType(8).TestValue);
			Assert.Equal(16, _registry.GetType(3).TestValue);
		}

		[Fact]
		public void LookupByType()
		{
			var typeA = Mock.Of<ITestType>(t => t.TestValue == 5);
			_registry.RegisterType(typeA, 8);
			var typeB = Mock.Of<ITestType>(t => t.TestValue == 16);
			_registry.RegisterType(typeB, 3);

			Assert.Equal(8, _registry.GetId(typeA));
			Assert.Equal(3, _registry.GetId(typeB));
		}

		[Fact]
		public void CannotLookupUnknown()
		{
			_registry.RegisterType(Mock.Of<ITestType>(), 12);

			Assert.Throws<InvalidOperationException>(() => { _registry.GetType(8); });
		}

		[Fact]
		public void CannotLookupRemoved()
		{
			var type = Mock.Of<ITestType>();
			_registry.RegisterType(type, 11);
			_registry.UnregisterType(type);

			Assert.Throws<InvalidOperationException>(() => { _registry.GetType(11); });
		}

		[Fact]
		public void CannotDoubleRegister()
		{
			_registry.RegisterType(Mock.Of<ITestType>(), 6);

			Assert.Throws<InvalidOperationException>(() => _registry.RegisterType(Mock.Of<ITestType>(), 6));
		}

		public interface ITestType
		{
			int TestValue { get; }
		}
	}
}