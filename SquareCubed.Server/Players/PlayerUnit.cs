using OpenTK;
using SquareCubed.Server.Structures;
using SquareCubed.Server.Units;
using SquareCubed.Server.Worlds;

namespace SquareCubed.Server.Players
{
	public class PlayerUnit : Unit
	{
		public PlayerUnit(Units.Units units) : base(units)
		{
		}

		public Player Player { get; set; }

		public override World World
		{
			get { return base.World; }
			set
			{
				base.World = value;
				if(Player.World != value)
					Player.World = value;
			}
		}

		/// <summary>
		///     Used on teleportation to lock the player position until
		///     the client has confirmed it has received the teleport.
		///		This is done to prevent player positon updates to override
		///		the teleport, resulting in a short unit flicker.
		///		TODO: Actually make the engine use this.
		/// </summary>
		public bool TeleportLocked { get; set; }

		public override void Teleport(ServerStructure targetStructure, Vector2 targetPosition)
		{
			TeleportLocked = true;
			base.Teleport(targetStructure, targetPosition);
		}
	}
}