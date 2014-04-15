using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		private World _world;
		private Structure _structure;

		public int Id { get; set; }

		public virtual World World
		{
			get { return _world; }
			set
			{
				// If already this, don't do anything
				if (value == _world) return;

				// Flip around the reference and keep a copy
				var oldWorld = _world;
				_world = value;

				// Update the entries in the worlds
				if (oldWorld != null) oldWorld.UpdateUnitEntry(this);
				if (_world != null) _world.UpdateUnitEntry(this);
			}
		}

		public Structure Structure
		{
			get { return _structure; }
			set
			{
				// If already this, don't do anything
				if (value == _structure) return;

				// Flip around the reference and keep a copy
				var oldStructure = _structure;
				_structure = value;

				// Update the entries in the worlds
				if (oldStructure != null) oldStructure.UpdateUnitEntry(this);
				if (_structure != null) _structure.UpdateUnitEntry(this);
			}
		}

		public Vector2 Position { get; set; }
	}
}