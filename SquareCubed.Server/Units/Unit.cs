using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Worlds;
using SquareCubed.Common.Utils;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		private readonly Units _units;

		public int Id { get; set; }

		public Unit(Units units)
		{
			_units = units;
			WorldLink = new ParentLink<World, Unit>(this, w => w.Units);
			StructureLink = new ParentLink<ServerStructure, Unit>(this, s => s.Units);
		}

		public ParentLink<World, Unit> WorldLink { get; private set; }

		public virtual World World
		{
			get { return WorldLink.Property; }
			set { WorldLink.Property = value; }
		}

		public ParentLink<ServerStructure, Unit> StructureLink { get; private set; }

		public ServerStructure Structure
		{
			get { return StructureLink.Property; }
			set { StructureLink.Property = value; }
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