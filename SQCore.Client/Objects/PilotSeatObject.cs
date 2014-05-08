using OpenTK;
using OpenTK.Input;
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

		private Vector2 _storedPos;
		private bool _hasPlayer;

		public PilotSeatObject(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_panel = panel;
			_player = client.Player;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new UnitProximityHelper(this);
			_proximity.Change += OnChange;
		}

		public Vector2 Position { get; set; }

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player.PlayerUnit);
		}

		void OnKeyPress(object sender, KeyboardKeyEventArgs e)
		{
			if (!_hasPlayer || e.Key != Key.Escape) return;

			_hasPlayer = false;

			_player.Position = _storedPos;

			_player.LockInput = false;
			_panel.UseAltText = false;
		}

		public void OnUse()
		{
			if (_hasPlayer || _proximity.Status != ProximityStatus.Within) return;

			_hasPlayer = true;

			_storedPos = _player.Position;
			_player.Position = Position;

			_player.LockInput = true;
			_panel.UseAltText = true;
		}

		private void OnChange(object s, ProximityEventArgs e)
		{
			// Actually do something with this data
			_panel.IsVisible = e.NewStatus == ProximityStatus.Within;
		}
	}
}