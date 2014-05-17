namespace SquareCubed.Server.Structures.Objects
{
	public interface IServerObjectType
	{
		ServerObjectBase CreateNew(ServerStructure parent);
	}
}
