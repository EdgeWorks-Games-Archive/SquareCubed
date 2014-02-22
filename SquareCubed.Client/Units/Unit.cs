using OpenTK;

namespace SquareCubed.Client.Units
{
	class Unit
	{
		public uint Id { get; private set; }
		public Vector2 Position { get; set; }

		public Unit(uint id)
		{
			Id = id;
		}
	}
}
