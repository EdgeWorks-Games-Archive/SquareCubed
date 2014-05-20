using OpenTK;

namespace SquareCubed.Client.Structures.Objects
{
	public abstract class ClientObjectBase
	{
		public int Id { get; set; }
		public Vector2 Position { get; set; }
		public ClientStructure Parent { get; private set; }

		protected ClientObjectBase(ClientStructure parent)
		{
			Parent = parent;
		}

		public abstract void OnUse();
	}
}