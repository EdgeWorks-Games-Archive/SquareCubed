using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectTypes
	{
		public Type[] TypeList { get; private set; }

		public ObjectTypes()
		{
			TypeList = new Type[20];
		}

		public void RegisterType(Type type, uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id < TypeList.Length,
				"Type Id is bigger than the amount of allocated type Id slots.");

			if (TypeList[id] != null)
				throw new Exception("Tile type already registered!");

			TypeList[id] = type;
		}
	}
}
