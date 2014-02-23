using OpenTK;

namespace SquareCubed.Client.Units
{
	public class Unit
	{
		public uint Id { get; private set; }
		public Vector2 Position { get; set; }

		public virtual void ProcessPhysicsPacketData(Vector2 position)
		{
			// Virtual because PlayerUnit will want to ignore server packets.
			Position = position;
		}

		public Unit(uint id)
		{
			Id = id;
		}
	}
}
