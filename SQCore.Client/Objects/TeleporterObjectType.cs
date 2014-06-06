using OpenTK.Input;
using SquareCubed.Client.Graphics;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal sealed class TeleporterObjectType : IClientObjectType
	{
		private readonly SquareCubed.Client.Client _client;

		public TeleporterObjectType(SquareCubed.Client.Client client)
		{
			_client = client;

			Texture = new Texture2D("./Graphics/Objects/Teleporter.png");

			_client.Input.TrackKey(Key.X);
		}

		public Texture2D Texture { get; private set; }

		public ClientObjectBase CreateNew(ClientStructure parent)
		{
			return new TeleporterObject(parent, this, _client);
		}

		public void Dispose()
		{
			Texture.Dispose();
		}
	}
}