using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Tiles;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;
		private readonly StructuresRenderer _renderer;
		private readonly Dictionary<int, ClientStructure> _structures = new Dictionary<int, ClientStructure>();

		public TypeRegistry<TileType> TileTypes { get; private set; }
		public TypeRegistry<IClientObjectType> ObjectTypes { get; private set; }
		public ObjectNetwork ObjectNetwork { get; private set; }

		public IEnumerable<ClientStructure> List
		{
			get { return _structures.Values; }
		}

		public ClientStructure GetOrNull(int id)
		{
			ClientStructure structure;
			return _structures.TryGetValue(id, out structure) ? structure : null;
		}

		internal Structures(Client client)
		{
			Contract.Requires<ArgumentNullException>(client != null);

			TileTypes = new TypeRegistry<TileType>();
			TileTypes.RegisterType(new InvisibleTileType(), 1);

			ObjectTypes = new TypeRegistry<IClientObjectType>();
			ObjectNetwork = new ObjectNetwork(client.Network);

			_network = new StructuresNetwork(client.Network, this);
			_renderer = new StructuresRenderer(client);
		}

		public void OnStructureData(ClientStructure structure)
		{
			Contract.Requires<ArgumentNullException>(structure != null);

			// Try to get the unit, if we can't we need to add it, otherwise overwrite it
			if (!_structures.ContainsKey(structure.Id))
				_structures.Add(structure.Id, structure);
			else
				_structures[structure.Id] = structure;
		}

		public void Render()
		{
			_renderer.RenderStructures(_structures.Values);
		}
	}
}