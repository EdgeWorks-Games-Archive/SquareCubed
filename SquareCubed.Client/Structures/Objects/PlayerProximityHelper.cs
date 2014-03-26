namespace SquareCubed.Client.Structures.Objects
{
	/// <summary>
	/// Helper class to act on a player's proximity to an object.
	/// </summary>
	public class PlayerProximityHelper
	{
		private ClientObject _obj;
		private Player.Player _player;

		public PlayerProximityHelper(ClientObject obj, Player.Player player)
		{
			_obj = obj;
			_player = player;
		}
	}
}
