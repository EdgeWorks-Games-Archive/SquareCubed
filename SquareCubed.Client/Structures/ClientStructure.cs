using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Units;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Structures
{
	public class ClientStructure : IComplexPositionable
	{
		private readonly List<Unit> _units = new List<Unit>();

		public int Id { get; set; }
		public List<ClientChunk> Chunks { get; set; }

		public IEnumerable<Unit> Units
		{
			get { return _units.AsReadOnly(); }
		}

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public Vector2 Center { get; set; }
		public List<IClientObject> Objects { get; set; }

		private void UpdateEntry<T>(ICollection<T> list, T entry, ClientStructure newStructure)
		{
			// If this world, add, if not, remove
			if (newStructure == this)
			{
				// Make sure it's not already in this world before adding
				if (!list.Contains(entry))
					list.Add(entry);
			}
			else
				list.Remove(entry);
		}

		public void UpdateUnitEntry(Unit unit)
		{
			Contract.Requires<ArgumentNullException>(unit != null);
			UpdateEntry(_units, unit, unit.Structure);
		}

		public IEnumerable<Chunk> GetChunksWithin(Vector2i centerChunkPos, int maxDistance)
		{
			return Chunks.Where(c =>
				(c.Position.X <= centerChunkPos.X + maxDistance) && (c.Position.X >= centerChunkPos.X - maxDistance) &&
				(c.Position.Y <= centerChunkPos.Y + maxDistance) && (c.Position.Y >= centerChunkPos.Y - maxDistance));
		}
	}

	public static class StructureExtensions
	{
		private static List<IClientObject> ReadObjects(this NetIncomingMessage msg, TypeRegistry<IClientObjectType> objectTypes, ClientStructure structure)
		{
			var amount = msg.ReadInt32();
			var objects = new List<IClientObject>(amount);
			for (var i = 0; i < amount; i++)
			{
				// Create an object of the type with the id we received assigned to it.
				var obj = objectTypes.GetType(msg.ReadInt32()).CreateNew(structure);
				obj.Id = msg.ReadInt32();
				obj.Position = msg.ReadVector2();
				objects.Add(obj);
			}

			return objects;
		}

		private static List<ClientChunk> ReadChunks(this NetIncomingMessage msg)
		{
			var amount = msg.ReadInt32();
			var chunks = new List<ClientChunk>(amount);
			for (var i = 0; i < amount; i++)
				chunks.Add(msg.ReadChunk());

			return chunks;
		}

		public static ClientStructure ReadStructure(this NetIncomingMessage msg, TypeRegistry<IClientObjectType> objectTypes)
		{
			Contract.Requires<ArgumentNullException>(msg != null);

			var structure = new ClientStructure
			{
				Id = msg.ReadInt32(),
				Position = msg.ReadVector2(),
				Rotation = msg.ReadFloat(),
				Center = msg.ReadVector2(),
				Chunks = msg.ReadChunks()
			};
			structure.Objects = msg.ReadObjects(objectTypes, structure);

			return structure;
		}
	}
}