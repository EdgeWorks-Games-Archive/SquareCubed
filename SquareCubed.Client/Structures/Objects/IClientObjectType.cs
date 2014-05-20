namespace SquareCubed.Client.Structures.Objects
{
	public interface IClientObjectType
	{
		IClientObject CreateNew(ClientStructure parent);
	}
}
