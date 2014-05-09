using Moq;
using OpenTK;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects.Components;
using Xunit;

namespace SquareCubed.Tests.Client.Structures.Objects.Components
{
	public class SeatTests
	{
		private readonly Seat _seat;
		private readonly IPlayer _player;

		public SeatTests()
		{
			// Set up a simple mock of the player object
			var playerMock = new Mock<IPlayer>();
			playerMock.SetupProperty(p => p.Position, new Vector2(0.5f, 1.0f));
			_player = playerMock.Object;

			// Set up the seat object
			_seat = new Seat(_player)
			{
				Position = new Vector2(2.0f, 2.5f)
			};
		}

		[Fact]
		public void SitSnapsToPosition()
		{
			_seat.Sit();
			Assert.Equal(_seat.Position, _player.Position);
		}

		[Fact]
		public void EmptyRestoresPosition()
		{
			var original = _player.Position;
			_seat.Sit();
			_seat.Empty();
			Assert.Equal(original, _player.Position);
		}
	}
}