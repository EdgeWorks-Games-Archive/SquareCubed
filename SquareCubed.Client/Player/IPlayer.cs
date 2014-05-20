using SquareCubed.Common.Data;

namespace SquareCubed.Client.Player
{
	public interface IPlayer : IParentable
	{
		bool LockInput { set; }
	}
}