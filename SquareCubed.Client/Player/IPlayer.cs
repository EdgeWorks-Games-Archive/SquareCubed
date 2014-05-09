using SquareCubed.Common.Data;

namespace SquareCubed.Client.Player
{
	public interface IPlayer : IPositionable
	{
		bool LockInput { set; }
	}
}