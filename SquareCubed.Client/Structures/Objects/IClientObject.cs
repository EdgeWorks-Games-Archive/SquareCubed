using OpenTK;

namespace SquareCubed.Client.Structures.Objects
{
	public interface IClientObject
	{
		int Id { get; set; }
		Vector2 Position { get; set; }

		void OnUse();
	}
}