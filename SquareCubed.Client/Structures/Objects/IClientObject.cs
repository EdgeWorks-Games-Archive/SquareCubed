using OpenTK;

namespace SquareCubed.Client.Structures.Objects
{
	public interface IClientObject
	{
		Vector2 Position { get; set; }

		void OnUse();
	}
}
