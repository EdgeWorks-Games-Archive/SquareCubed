using System;
using System.Diagnostics.Contracts;

namespace SquareCubed.Client.Structures.Objects
{
	public class ObjectTypes
	{
		private const uint MaxId = 20;
		private readonly Client _client;
		private readonly Type[] _typeList;

		public ObjectTypes(Client client)
		{
			_client = client;
			_typeList = new Type[MaxId + 1];
		}

		public Type GetType(uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");

			return _typeList[id];
		}

		public ClientObject InstantiateType(uint id)
		{
			Contract.Requires<ArgumentOutOfRangeException>(
				id <= MaxId,
				"Object Id is bigger than the maximum Id allowed.");
			
			var type = GetType(id);

			// Make sure we actually managed to retrieve a type
			if(type == null)
				throw new Exception("Object type " + id + " not registered!");

			return (ClientObject) Activator.CreateInstance(type, _client);
		}

		public void RegisterType(Type type, uint id)
		{
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
	}
}