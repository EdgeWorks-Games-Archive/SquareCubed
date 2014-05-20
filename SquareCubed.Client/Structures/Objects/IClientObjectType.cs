namespace SquareCubed.Client.Structures.Objects
{
	public interface IClientObjectType
	{
		ClientObjectBase CreateNew(ClientStructure parent);
	}
}
