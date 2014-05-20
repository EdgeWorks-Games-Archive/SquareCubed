using OpenTK;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Structures.Objects
{
	public abstract class ClientObjectBase : IParentable
	{
		public int Id { get; set; }
		public Vector2 Position { get; set; }
		public IPositionable Parent { get; private set; }

		protected ClientObjectBase(ClientStructure parent)
		{
			Parent = parent;
		}

		public abstract void OnUse();
	}
}