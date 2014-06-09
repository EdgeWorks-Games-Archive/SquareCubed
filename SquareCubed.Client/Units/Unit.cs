using System.Diagnostics;
using OpenTK;
using SquareCubed.Client.Structures;
using SquareCubed.Common.Data;
using SquareCubed.Common.Utils;

namespace SquareCubed.Client.Units
{
	public class Unit : IParentable
	{
		public Unit(int id)
		{
			StructureLink = new ParentLink<ClientStructure, Unit>(this, s => s.Units);
			
			Id = id;
		}

		protected Unit(Unit oldUnit)
			: this(oldUnit.Id)
		{
			Debug.Assert(oldUnit != null);
			
			Position = oldUnit.Position;
			Structure = oldUnit.Structure;
		}

		public ParentLink<ClientStructure, Unit> StructureLink { get; private set; }

		public ClientStructure Structure
		{
			get { return StructureLink.Property; }
			set { StructureLink.Property = value; }
		}

		public int Id { get; private set; }
		public Vector2 Position { get; set; }

		public IPositionable Parent
		{
			get { return Structure; }
		}

		public virtual void ProcessPhysicsPacketData(Vector2 position)
		{
			// Virtual because PlayerUnit will want to ignore server packets.
			Position = position;
		}
	}
}