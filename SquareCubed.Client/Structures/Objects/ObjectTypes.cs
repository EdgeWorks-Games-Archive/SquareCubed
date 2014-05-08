using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectTypes
	{
		private const uint MaxId = 20;
		private readonly IObjectType[] _typeList;

		public ObjectTypes(Client client)
		{
			_typeList = new IObjectType[MaxId + 1];
		}

		public IObjectType GetType(int id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(id >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");

			return _typeList[id];
		}

		public void RegisterType(IObjectType type, int id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(id >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");

			// Overwriting the type could lead to serious issues.
			// Preventing the type from being overwritten even at
			// runtime will give us a clearer bugreport.
			if (_typeList[id] != null)
				throw new Exception("Object type " + id + " already registered!");

			_typeList[id] = type;
		}

		public void UnregisterType(IObjectType type)
		{
			var index = Array.IndexOf(_typeList, type);
			_typeList[index] = null;
		}
	}
}