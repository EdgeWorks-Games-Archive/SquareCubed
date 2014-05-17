using OpenTK;

namespace SquareCubed.Server.Structures.Objects
{
	public abstract class ServerObjectBase
	{
		public int Id { get; set; }
		public Vector2 Position { get; set; }
		public IServerObjectType Type { get; set; }

		static private int _idCounter;

		protected ServerObjectBase(IServerObjectType type)
		{
			Id = _idCounter++;
			Type = type;
		}
	}

	public abstract class NetworkServerObjectBase : ServerObjectBase
	{
		protected NetworkServerObjectBase(IServerObjectType type, ObjectsNetwork network) : base(type)
		{
			//network.Register();
		}
	}
}
