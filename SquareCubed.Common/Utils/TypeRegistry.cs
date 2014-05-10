using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Common.Utils
{
	public class TypeRegistry<TType> where TType : class
	{
		private const uint MaxId = 20;
		private readonly TType[] _typeList;

		public TypeRegistry()
		{
			_typeList = new TType[MaxId + 1];
		}

		public TType GetType(int id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(id >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object TypeId is bigger than the maximum TypeId allowed.");

			return _typeList[id];
		}

		public void RegisterType(TType type, int id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(id >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object TypeId is bigger than the maximum TypeId allowed.");

			// Overwriting the type could lead to serious issues.
			// Preventing the type from being overwritten even at
			// runtime will give us a clearer bugreport.
			if (_typeList[id] != null)
				throw new Exception("Object type " + id + " already registered!");

			_typeList[id] = type;
		}

		public void UnregisterType(TType type)
		{
			var index = Array.IndexOf(_typeList, type);
			_typeList[index] = null;
		}
	}
}