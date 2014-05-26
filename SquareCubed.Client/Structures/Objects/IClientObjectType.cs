using System;

namespace SquareCubed.Client.Structures.Objects
{
	public interface IClientObjectType : IDisposable
	{
		ClientObjectBase CreateNew(ClientStructure parent);
	}
}
