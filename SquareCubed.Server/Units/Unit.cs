using Lidgren.Network;
using OpenTK;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Units
{
	public class Unit
	{
		public uint Id { get; set; }
		public virtual World World { get; set; }
		public Vector2 Position { get; set; }

		public Unit(World world, Vector2 position)
		{
			World = world;
			Position = position;
		}

		public void WritePositionData(NetOutgoingMessage msg)
		{
			msg.Write(Position.X);
			msg.Write(Position.Y);
		}
	}
}
