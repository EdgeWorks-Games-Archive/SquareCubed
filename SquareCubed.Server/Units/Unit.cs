using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Worlds;
using SquareCubed.Common.Utils;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		private readonly Units _units;
		private ServerStructure _structure;

		public int Id { get; set; }

		public Unit(Units units)
		{
			_units = units;
			WorldLink = new ParentLink<World, Unit>(this, w => w.Units);
		}

		public ParentLink<World, Unit> WorldLink { get; private set; }
		public virtual World World
		{
			get { return WorldLink.Property; }
			set { WorldLink.Property = value; }
		}

		public ServerStructure Structure
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

		public virtual void Teleport(ServerStructure targetStructure, Vector2 targetPosition)
		{
			Structure = targetStructure;
			Position = targetPosition;
			_units.SendTeleportFor(this);
		}
	}
}