using System;
using Lidgren.Network;
using OpenTK;
using OpenTK.Input;
using SQCore.Client.Gui;
using SquareCubed.Client;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Objects.Components;
using SquareCubed.Network;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : IClientObject
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ContextInfoPanel _panel;
		private readonly UnitProximityHelper _proximity;
		private readonly Seat _seat;

		private Vector2 _position;
		private float _throttle;

		public PilotSeatObject(SquareCubed.Client.Client client, ContextInfoPanel panel, ClientStructure parent)
		{
			_client = client;
			_panel = panel;
			Parent = parent;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new UnitProximityHelper(this);
			_proximity.Change += OnProximityChange;

			_seat = new Seat(_client.Player);
			_seat.PlayerSits += OnPlayerSits;
			_seat.PlayerExits += OnPlayerExits;
		}

		private void OnPlayerSits(object sender, EventArgs e)
		{
			_client.Graphics.Camera.PilotMode = true;
		}

		private void OnPlayerExits(object sender, EventArgs e)
		{
			_client.Graphics.Camera.PilotMode = false;
		}

		public int Id { get; set; }

		public Vector2 Position
		{
			get { return _position; }
			set
			{
				_position = value;
				_seat.Position = value;
			}
		}

		public ClientStructure Parent { get; set; }

		public void OnUse()
		{
			if (_seat.HasPlayer || _proximity.Status != ProximityStatus.Within) return;

			_seat.Sit();
			_panel.Text = "Press Esc to Exit";
		}

		private void Update(object s, TickEventArgs e)
		{
			// Update the proximity helper, if there's no player it will default to not within range
			_proximity.Update(_client.Player);

			// If the player isn't in the seat, we're done
			if (!_seat.HasPlayer)
				return;

			if (_client.Input.GetKey(Key.W) && !_client.Input.GetKey(Key.S))
			{
				// Shift increases throttle
				if (_throttle < 1.0f)
					_throttle += 0.5f*e.ElapsedTime;
				if (_throttle > 1.0f)
					_throttle = 1.0f;
			}
			else if (_client.Input.GetKey(Key.S))
			{
				// Control decreases throttle
				if (_throttle > 0.0f)
					_throttle -= 0.5f*e.ElapsedTime;
				if (_throttle < 0.0f)
					_throttle = 0.0f;
			}

			// X cuts throttle
			if (_client.Input.GetKey(Key.X))
				_throttle = 0.0f;

			// A and D are angular throttle, this will later be replaced with RCS
			var angularThrottle = 0.0f;
			if (_client.Input.GetKey(Key.A) && ! _client.Input.GetKey(Key.D))
				angularThrottle = -0.08f;
			else if (_client.Input.GetKey(Key.D))
				angularThrottle = 0.08f;

			// Send throttle update to the server
			var msg = _client.Structures.ObjectsNetwork.CreateMessageFor(this);
			msg.Write(_throttle);
			msg.Write(angularThrottle);
			_client.Network.SendToServer(msg, NetDeliveryMethod.ReliableSequenced, (int) SequenceChannels.PilotUpdate);
		}

		private void OnKeyPress(object sender, KeyboardKeyEventArgs e)
		{
			if (!_seat.HasPlayer || e.Key != Key.Escape) return;

			_seat.Empty();
			_panel.Text = "Click to Interact";
		}

		private void OnProximityChange(object s, ProximityEventArgs e)
		{
			if (e.NewStatus == ProximityStatus.Within)
			{
				_panel.Text = "Click to Interact";
				_panel.IsVisible = true;
			}
			else
				_panel.IsVisible = false;
		}
	}
}