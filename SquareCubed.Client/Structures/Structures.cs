using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Tiles;

namespace SquareCubed.Client.Structures
{
	public class Structures
	{
		private readonly StructuresNetwork _network;
		private readonly StructuresRenderer _renderer;
		private readonly Dictionary<int, Structure> _structures = new Dictionary<int, Structure>();

		public TileTypes TileTypes { get; private set; }
		public ObjectTypes ObjectTypes { get; private set; }

		public IEnumerable<Structure> List
		{
			get { return _structures.Values; }
		}

		public Structure GetOrNull(int id)
		{
			Structure structure;
			return _structures.TryGetValue(id, out structure) ? structure : null;
		}

		public Structures(Client client)
		{
			TileTypes = new TileTypes();
			ObjectTypes = new ObjectTypes(client);

			_network = new StructuresNetwork(client, this);
			_renderer = new StructuresRenderer(client);
		}

		public void OnStructureData(Structure structure)
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