using System;
using OpenTK;
using SquareCubed.Client.Player;

namespace SquareCubed.Client.Structures.Objects.Components
{
	public class Seat
	{
		public bool HasPlayer { get; private set; }
		public Vector2 Position { get; set; }

		private readonly IPlayer _player;
		private Vector2 _storedPos;

		public event EventHandler PlayerSits = (s, a) => { };
		public event EventHandler PlayerExits = (s, a) => { };

		public Seat(IPlayer player)
		{
			_player = player;
		}

		public void Sit()
		{
			// If we have the player we can't get him there again
			if (HasPlayer) return;

			HasPlayer = true;
			_storedPos = _player.Position;
			_player.Position = Position;
			_player.LockInput = true;
			PlayerSits(this, EventArgs.Empty);
		}

		public void Empty()
		{
			// If we don't have a player we can't empty
			if (!HasPlayer) return;

			HasPlayer = false;
			_player.Position = _storedPos;
			_player.LockInput = false;
			PlayerExits(this, EventArgs.Empty);
		}
	}
}
