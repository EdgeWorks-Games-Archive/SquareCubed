using SQCore.Common;
using SquareCubed.Client;

namespace SQCore.Client
{
	public class ClientPlugin : CommonPlugin, IClientPlugin
	{
		private readonly Player _player;

		public ClientPlugin(SquareCubed.Client.Client client)
		{
			Logger.LogInfo("Initializing core plugin...");

			_player = new Player(client);

			client.UpdateTick += OnUpdate;
			client.UnitRenderTick += OnRender;

			Logger.LogInfo("Finished initializing core plugin!");
		}

		private void OnUpdate(object sender, float delta)
		{
			_player.Update(delta);
		}

		private void OnRender(object sender, float delta)
		{
			_player.Render(delta);
		}
	}
}