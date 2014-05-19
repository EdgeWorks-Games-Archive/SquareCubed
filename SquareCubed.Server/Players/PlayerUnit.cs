using OpenTK;
using SquareCubed.Server.Structures;
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

		/// <summary>
		///     Used on teleportation to lock the player position until
		///     the client has confirmed it has received the teleport.
		///		This is done to prevent player positon updates to override
		///		the teleport, resulting in a short unit flicker.
		/// </summary>
		public bool TeleportLocked { get; set; }

		public override void Teleport(ServerStructure targetStructure, Vector2 targetPosition)
		{
			TeleportLocked = true;
			base.Teleport(targetStructure, targetPosition);
		}
	}
}