using OpenTK;
using SquareCubed.Client.Units;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Player
{
	public class PlayerUnit : Unit
	{
		public AaBb AaBb
		{
			get
			{
				return new AaBb
				{
					Position = Position - new Vector2(0.3f, 0.3f),
					Size = new Vector2(0.6f, 0.6f)
				};
			}
		}

		public override void ProcessPhysicsPacketData(Vector2 position)
		{
			// Player doesn't care about update packets
		}

		public PlayerUnit(Unit oldUnit)
			: base(oldUnit)
		{
		}
	}
}
