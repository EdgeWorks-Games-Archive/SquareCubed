using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Client.Units;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Structures
{
	public class Structure : IPositionable
	{
		private readonly List<Unit> _units = new List<Unit>();

		public uint Id { get; set; }
		public List<Chunk> Chunks { get; set; }

		public IEnumerable<Unit> Units
		{
			get { return _units.AsReadOnly(); }
		}

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public Vector2 Center { get; set; }

		private void UpdateEntry<T>(ICollection<T> list, T entry, Structure newStructure)
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
		private static List<Chunk> ReadChunks(this NetIncomingMessage msg)
		{
			var amount = msg.ReadInt32();
			var chunks = new List<Chunk>(amount);
			for (var i = 0; i < amount; i++)
				chunks.Add(msg.ReadChunk());

			return chunks;
		}

		public static Structure ReadStructure(this NetIncomingMessage msg)
		{
			Contract.Requires<ArgumentNullException>(msg != null);

			return new Structure
			{
				Id = msg.ReadUInt32(),
				Position = msg.ReadVector2(),
				Rotation = msg.ReadFloat(),
				Center = msg.ReadVector2(),
				Chunks = msg.ReadChunks()
			};
		}
	}
}