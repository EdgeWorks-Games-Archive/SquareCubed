using OpenTK;

namespace SquareCubed.Data
{
	public interface IParentable
	{
		Vector2 Center { get; }
		Vector2 Position { get; }
		float Rotation { get; }
	}
}