using OpenTK;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		private World _world;

		public Unit(World world, Vector2 position)
		{
			_world = world;
			Position = position;

			// Set and Configure Unit Data
			World.UpdateUnitEntry(this);
		}

		public uint Id { get; set; }

		public virtual World World
		{
			get { return _world; }
			set
			{
				var oldWorld = value;
				_world = value;

				oldWorld.UpdateUnitEntry(this);
				_world.UpdateUnitEntry(this);
			}
		}

		public Vector2 Position { get; set; }
	}
}