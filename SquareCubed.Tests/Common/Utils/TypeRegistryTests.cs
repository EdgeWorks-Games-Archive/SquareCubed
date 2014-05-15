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
		public void Lookup()
		{
			_registry.RegisterType(Mock.Of<ITestType>(t => t.TestValue == 5), 8);
			_registry.RegisterType(Mock.Of<ITestType>(t => t.TestValue == 16), 3);

			Assert.Equal(5, _registry.GetType(8).TestValue);
			Assert.Equal(16, _registry.GetType(3).TestValue);
		}

		[Fact]
		public void CannotLookupUnregistered()
		{
			_registry.RegisterType(Mock.Of<ITestType>(), 12);

			Assert.Throws<InvalidOperationException>(() => { _registry.GetType(8); });
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