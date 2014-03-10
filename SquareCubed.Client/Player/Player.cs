using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SquareCubed.Common.Data;

namespace SquareCubed.Client.Player
{
	public class Player
	{
		private const float Speed = 2;
		private readonly Client _client;
		private readonly PlayerNetwork _network;
		private PlayerUnit _playerUnit;

		public Player(Client client)
		{
			_client = client;
			_network = new PlayerNetwork(_client, this);
		}

		public void OnPlayerData(uint id)
		{
			var unit = _client.Units.GetAndRemove(id);
			_playerUnit = new PlayerUnit(unit);
			_client.Units.Add(_playerUnit);

			// Update the old unit's structure entry so it isn't kept alive by the structure
			// TODO: Improve this to be done better somehow
			unit.Structure = null;
		}

		public void Update(float delta)
		{
			// Make sure the player is correctly set up, if not just ignore this tick
			if (_playerUnit == null) return;
			if (_playerUnit.Structure == null) return;

			// Calculate movement data before doing anything
			var velocity = _client.Input.Axes*delta*Speed;
			var xHasVel = (Math.Abs(velocity.X) > 0.001);
			var yHasVel = (Math.Abs(velocity.Y) > 0.001);

			// If there's velocity, we need to do stuff
			if (xHasVel || yHasVel)
			{
				// Calculate additional movement data
				var position = _playerUnit.Position;
				var newPosition = position + velocity;

				// Get the collider data we'll need to detect collisions
				var colliders = _playerUnit.Structure.GetChunksWithin(newPosition.GetChunkPosition(), 1).SelectMany(c => c.GetColliders());

				// If there's vertical movement
				if (xHasVel)
				{
					// If the player is going left
					if (velocity.X < 0)
					{
						// Create the player's left side
						var playerSide = new AaSide
						{
							Length = 0.6f,
							CenterDistance = 0.3f
						};

						// Get all the sides that are collidable on the Y axis (which won't change)
						var sides = colliders.Select(c => c.Right).Where(s =>
							// Only include where the player's highest point is above the lowest point of the side
							(playerSide.Position.Y + playerSide.Length > s.Position.Y) &&
							// Only include where the player's lowest point is below the highest point of the side
							(playerSide.Position.Y < s.Position.Y + s.Length));

						// ReSharper disable PossibleMultipleEnumeration
						while(true)
						{
							// Update the position in the player side
							playerSide.Position = newPosition - new Vector2(0.3f, 0.3f);

							// Find the first side that meets the collision requirements
							var side = sides.FirstOrDefault(s =>
								// Only include where the player's left side side is left of the side
								(playerSide.Position.X < s.Position.X) &&
								// Only include where the player's left side + center distance is right of the side - center distance
								(playerSide.Position.X + playerSide.CenterDistance > s.Position.X - s.CenterDistance));

							if (side != null)
							{
								// Calculate difference in target position needed to resolve collision
								var diffVel = playerSide.Position.X - side.Position.X;
								
								// Remove difference from velocity and update new position
								velocity.X -= diffVel;
								newPosition.X = position.X + velocity.X;
							}
							else break;
						}
						// ReSharper restore PossibleMultipleEnumeration
					}
				}

				// If there's horizontal movement
				if (yHasVel)
				{
				}

				// Update the player's position
				_playerUnit.Position = newPosition;

				// Send over the updated player position
				_network.SendPlayerPhysics(_playerUnit);
			}

			// Make sure the camera is parented correctly
			_client.Graphics.Camera.Parent = _playerUnit.Structure;
			_client.Graphics.Camera.Position = _playerUnit.Position;
		}

		public void Render()
		{
			// TODO: Move to structure and add debugging flag on the structures

			// Make sure the player is correctly set up, if not just ignore this tick
			/*if (_playerUnit == null) return;
			if (_playerUnit.Structure == null) return;

			GL.PushMatrix();
			GL.Translate(_playerUnit.Structure.Position.X, _playerUnit.Structure.Position.Y, 0);
			GL.Rotate(-_playerUnit.Structure.Rotation, 0, 0, 1);
			GL.Translate(-_playerUnit.Structure.Center.X, -_playerUnit.Structure.Center.Y, 0);

			// Render all AaBbs
			foreach (var collider in _playerUnit.Structure.GetChunksWithin(_playerUnit.Position.GetChunkPosition(), 1).SelectMany(c => c.GetColliders()))
			{
				GL.PushMatrix();
				GL.Translate(collider.Position.X, collider.Position.Y, 0);

				GL.Begin(PrimitiveType.LineLoop);

				GL.Color3(Color.Blue);

				GL.Vertex2(0.0f, 0.0f);
				GL.Vertex2(collider.Size.X, 0.0f);
				GL.Vertex2(collider.Size.X, collider.Size.Y);
				GL.Vertex2(0.0f, collider.Size.Y);

				GL.End();

				GL.PopMatrix();
			}

			GL.PopMatrix();*/
		}
	}
}