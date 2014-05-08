using OpenTK;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : IClientObject
	{
		private readonly ContextInfoPanel _panel;
		private readonly Player _player;
		private readonly UnitProximityHelper _proximity;

		public PilotSeatObject(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_panel = panel;
			_player = client.Player;
			_proximity = new UnitProximityHelper(this);

			client.UpdateTick += Update;
			_proximity.Change += OnChange;
		}

		public Vector2 Position { get; set; }

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player.PlayerUnit);
		}

		public void OnUse()
		{
			_player.PlayerUnit.Position = Position;
			_player.LockPosition = true;
			_panel.UseAltText = true;
		}

		private void OnChange(object s, ProximityEventArgs e)
		{
			// Actually do something with this data
			_panel.IsVisible = e.NewStatus == ProximityStatus.Within;
		}
	}
}