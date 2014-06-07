using System;
using Lidgren.Network;
using OpenTK.Input;
using SquareCubed.Client;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Structures.Objects.Components;
using SquareCubed.Network;

namespace SQCore.Client.Objects
{
	internal class PilotSeatObject : ClientObjectBase
	{
		private readonly SquareCubed.Client.Client _client;
		private readonly ProximityHelper _proximity;
		private readonly Seat _seat;
		private float _throttle;

		public PilotSeatObject(SquareCubed.Client.Client client, ClientStructure parent)
			: base(parent)
		{
			_client = client;

			client.UpdateTick += Update;
			client.Window.KeyUp += OnKeyPress;

			_proximity = new ProximityHelper(this);

			_seat = new Seat(this, _client.Player);
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

		public override void OnUse()
		{
			if (_seat.HasPlayer || _proximity.Status != ProximityStatus.Within) return;

			_seat.Sit();
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
		}
	}
}