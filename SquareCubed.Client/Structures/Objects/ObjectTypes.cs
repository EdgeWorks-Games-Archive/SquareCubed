using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectTypes
	{
		public const uint MaxId = 20;
		private Type[] _typeList;

		public ObjectTypes()
		{
			_typeList = new Type[MaxId + 1];
		}

		public Type GetType(uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");

			return _typeList[id];
		}

		public void RegisterType(Type type, uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");

			if (_typeList[id] != null)
				throw new Exception("Object type already registered!");

			_typeList[id] = type;
		}
	}
}
