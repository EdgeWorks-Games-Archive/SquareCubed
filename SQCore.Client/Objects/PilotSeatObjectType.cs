using OpenTK.Input;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal sealed class PilotSeatObjectType : IClientObjectType
	{
		private readonly SquareCubed.Client.Client _client;

		public PilotSeatObjectType(SquareCubed.Client.Client client)
		{
			_client = client;

			_client.Input.TrackKey(Key.X);
		}

		public ClientObjectBase CreateNew(ClientStructure parent)
		{
			return new PilotSeatObject(_client, parent);
		}

		public void Dispose()
		{
		}
	}
}