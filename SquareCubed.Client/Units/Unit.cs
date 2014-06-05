using System;
using System.Diagnostics;
using OpenTK;
using SquareCubed.Client.Structures;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Units
{
	public class Unit : IParentable
	{
		private ClientStructure _structure;

		public ClientStructure Structure
		{
			get { return _structure; }
			set
			{
				// Unit can not be in a structure on the client side.
				// For example if we're awaiting structure data to be downloaded.
				// TODO: For such a case, make sure the unit is linked to the structure as soon as it's downloaded.

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

		public int Id { get; private set; }
		public Vector2 Position { get; set; }
		public IPositionable Parent { get { return _structure; } }

		public virtual void ProcessPhysicsPacketData(Vector2 position)
		{
			// Virtual because PlayerUnit will want to ignore server packets.
			Position = position;
		}

		public Unit(int id)
		{
			Id = id;
		}

		protected Unit(Unit oldUnit)
		{
			Debug.Assert(oldUnit != null);

			Id = oldUnit.Id;
			Position = oldUnit.Position;
			Structure = oldUnit.Structure;
		}
	}
}
