using OpenTK;

namespace SquareCubed.Common.Data
{
	public interface IPositionable
	{
		Vector2 Position { get; set; }
	}

	public interface IParentable : IPositionable
	{
		IPositionable Parent { get; }
	}

	public interface IComplexPositionable : IPositionable
	{
		Vector2 Center { get; set; }
		float Rotation { get; set; }
	}
}