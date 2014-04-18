using System;
using System.Collections.Generic;
using System.Linq;

namespace SquareCubed.Network
{
	public class PacketTypes
	{
		public class PacketType
		{
			public PacketType(string name, int id)
			{
				Name = name;
				Id = id;
			}

			public string Name { get; private set; }
			public int Id { get; private set; }
		}

		private readonly Dictionary<string, PacketType> _packetTypes = new Dictionary<string, PacketType>();
		private int _nextId = 0;

		public PacketType RegisterType(string name)
		{
			return RegisterType(name, _nextId);
		}

		public PacketType RegisterType(string name, int id)
		{
			// Chech requirements
			if(_packetTypes.ContainsKey(name))
				throw new InvalidOperationException("Type name already registered!");
			if(_packetTypes.Any(p => p.Value.Id == id))
				throw new InvalidOperationException("Type id already registered!");

			// Create and add
			var type = new PacketType(name, id);
			_packetTypes.Add(name, type);

			// Increment nextId if needed
			if (_nextId <= id)
				_nextId = id + 1;

			return type;
		}

		public PacketType ResolveType(string name)
		{
			PacketType type;
			if (!_packetTypes.TryGetValue(name, out type))
				throw new InvalidOperationException("Type name not registered!");
			return type;
		}
	}
}
