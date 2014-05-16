using OpenTK;

namespace SquareCubed.Server.Structures.Objects
{
	public abstract class ServerObjectBase
	{
		/// <summary>Position of the object relative to the chunk.</summary>
		public Vector2 Position { get; set; }
		public IServerObjectType Type { get; set; }

		protected ServerObjectBase(IServerObjectType type)
		{
			Type = type;
		}
	}
}
