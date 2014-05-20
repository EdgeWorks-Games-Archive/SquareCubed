using Moq;
using OpenTK;
using SquareCubed.Client.Player;
using SquareCubed.Client.Structures.Objects.Components;
using SquareCubed.Common.Data;
using Xunit;

namespace SquareCubed.Tests.Client.Structures.Objects.Components
{
	public class SeatTests
	{
		private readonly Seat _seat;
		private readonly IPositionable _object;
		private readonly IPlayer _player;

		public SeatTests()
		{
			// Set up mocks
			_player = Mock.Of<IPlayer>(p => p.Position == new Vector2(0.5f, 1.0f));
			_object = Mock.Of<IPositionable>(o => o.Position == new Vector2(2.0f, 2.5f));

			// Set up the seat object
			_seat = new Seat(_object, _player);
		}

		[Fact]
		public void SitSnapsToPosition()
		{
			_seat.Sit();
			Assert.Equal(_object.Position, _player.Position);
		}

		[Fact]
		public void ObjectMoveMovesSitPosition()
		{
			_object.Position = new Vector2(5.0f, 2.4f);
			_seat.Sit();
			Assert.Equal(_object.Position, _player.Position);
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