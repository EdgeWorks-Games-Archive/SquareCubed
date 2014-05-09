using OpenTK;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Objects.Components;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : IClientObject
	{
		private readonly ContextInfoPanel _panel;
		private readonly IPlayer _player;
		private readonly UnitProximityHelper _proximity;
		private readonly Seat _seat;

		private Vector2 _position;

		public PilotSeatObject(SquareCubed.Client.Client client, ContextInfoPanel panel)
		{
			_panel = panel;
			_player = client.Player;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new UnitProximityHelper(this);
			_proximity.Change += OnChange;

			_seat = new Seat(_player);
		}

		public Vector2 Position
		{
			get { return _position; }
			set
			{
				_position = value;
				_seat.Position = value;
			}
		}

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_player);
		}

		void OnKeyPress(object sender, KeyboardKeyEventArgs e)
		{
			if (!_seat.HasPlayer || e.Key != Key.Escape) return;

			_seat.Empty();
			_panel.UseAltText = false;
		}

		public void OnUse()
		{
			if (_seat.HasPlayer || _proximity.Status != ProximityStatus.Within) return;

			_seat.Sit();
			_panel.UseAltText = true;
		}

		private void OnChange(object s, ProximityEventArgs e)
		{
			// Actually do something with this data
			_panel.IsVisible = e.NewStatus == ProximityStatus.Within;
		}
	}
}