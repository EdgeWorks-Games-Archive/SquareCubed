using OpenTK;
using SquareCubed.Server.Units;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Players
{
	public class PlayerUnit : Unit
	{
		public Player Player { get; set; }

		public override World World
		{
			get { return base.World; }
			set
			{
				var oldWorld = value;
				base.World = value; // Will also update unit world entries

				// If player has been set, update the
				// world references to the player.
				if (Player == null) return;
				oldWorld.UpdatePlayerEntry(Player);
				base.World.UpdatePlayerEntry(Player);
			}
		}
	}
}