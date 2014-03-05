using System;
using System.Diagnostics.Contracts;
using OpenTK;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		private World _world;

		public uint Id { get; set; }

		public virtual World World
		{
			get { return _world; }
			set
			{
				// Unit must always be in a world
				Contract.Requires<ArgumentNullException>(value != null);

				// If already this, don't do anything
				if (value == _world) return;

				// Flip around the reference and keep a copy
				var oldWorld = _world;
				_world = value;

				// Update the entries in the worlds
				if (oldWorld != null) oldWorld.UpdateUnitEntry(this);
				_world.UpdateUnitEntry(this);
			}
		}

		public Vector2 Position { get; set; }
	}
}