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
				// If already this, don't do anything
				if (value == base.World) return;

				var oldWorld = base.World;
				base.World = value; // Will also update unit world entries

				// If player has not been set, don't update world references yet
				if (Player == null) return;

				if (oldWorld != null)
					oldWorld.UpdatePlayerEntry(Player);
				if (base.World != null)
					base.World.UpdatePlayerEntry(Player);
			}
		}
	}
}