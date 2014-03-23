using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lidgren.Network;
using OpenTK;
using SquareCubed.Common.Data;
using SquareCubed.Server.Units;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Structures
{
	public class Structure
	{
		private readonly List<Unit> _units = new List<Unit>();
		private World _world;

		public Structure()
		{
			Chunks = new List<ServerChunk>();
		}

		public World World
		{
			get { return _world; }
			set
			{
				var oldWorld = _world;
				_world = value;

				// Update links in worlds
				if (oldWorld != null)
					oldWorld.UpdateStructureEntry(this);
				if (_world != null)
					_world.UpdateStructureEntry(this);
			}
		}

		public uint Id { get; set; }
		public List<ServerChunk> Chunks { get; set; }
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }

		/// <summary>
		///     The location in the chunk data where the center of the structure is.
		///     This is the axis the structure rotates around and thus is the center of mass.
		/// </summary>
		public Vector2 Center { get; set; }

		public IEnumerable<Unit> Units
		{
			get { return _units.AsReadOnly(); }
		}

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
	}

	public static class StructureExtensions
	{
		public static void Write(this NetOutgoingMessage msg, Structure structure)
		{
			Contract.Requires<ArgumentNullException>(msg != null);
			Contract.Requires<ArgumentNullException>(structure != null);

			// Add metadata and position
			msg.Write(structure.Id);
			msg.Write(structure.Position);
			msg.Write(structure.Rotation);
			msg.Write(structure.Center);

			// Add structure chunk data
			msg.Write(structure.Chunks.Count);
			foreach (var chunk in structure.Chunks)
				msg.Write(chunk);
		}
	}
}