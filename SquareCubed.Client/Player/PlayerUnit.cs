using OpenTK;
using SquareCubed.Client.Units;

namespace SquareCubed.Client.Player
{
	class PlayerUnit : Unit
	{
		public override void ProcessPhysicsPacketData(Vector2 position)
		{
			// Player doesn't care about update packets
		}

		public PlayerUnit(Unit oldUnit)
			: base(oldUnit.Id)
		{
		}
	}
}
