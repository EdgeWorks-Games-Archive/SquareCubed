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

		public Seat(IPlayer player)
		{
			_player = player;
		}

		public void Sit()
		{
			HasPlayer = true;
			_storedPos = _player.Position;
			_player.Position = Position;
			_player.LockInput = true;
		}

		public void Empty()
		{
			HasPlayer = false;
			_player.Position = _storedPos;
			_player.LockInput = false;
		}
	}
}
