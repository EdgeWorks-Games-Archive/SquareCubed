using OpenTK;

namespace SquareCubed.Client.Player
{
	public interface IPlayer
	{
		Vector2 Position { get; set; }
		bool LockInput { set; }

		// TODO: Change to an interface that has a Position property
		PlayerUnit WorldPlayer { get; }
	}
}