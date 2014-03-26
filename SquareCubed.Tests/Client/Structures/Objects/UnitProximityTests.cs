using OpenTK;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Units;
using Xunit;

namespace SquareCubed.Tests.Client.Structures.Objects
{
	public class UnitProximityTests
	{
		private readonly UnitProximityHelper _proximity;

		public UnitProximityTests()
		{
			var obj = new ClientObject
			{
				// Initial position of the object
				Position = new Vector2(2.0f, 3.5f)
			};

			_proximity = new UnitProximityHelper(obj)
			{
				// Initial range of the proximity detection
				Range = 1.0f
			};
		}

		[Fact]
		public void InitiallyIsNotWithin()
		{
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);
		}

		[Fact]
		public void NullUpdatesToNotWithin()
		{
			_proximity.Update(null);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);
		}

		[Fact]
		public void DetectedInitialInside()
		{
			var unit = new Unit(0)
			{
				// Should be within 1 distance of the test object
				Position = new Vector2(1.8f, 3.6f)
			};
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.Within, _proximity.Status);
		}

		[Fact]
		public void DetectedInitialOutside()
		{
			var unit = new Unit(0)
			{
				// Should be not within 1 distance of the test object
				Position = new Vector2(1.0f, 3.7f)
			};
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);
		}

		[Fact]
		public void DetectedInsideAfterRangeChange()
		{
			var unit = new Unit(0)
			{
				// Should be not within 1 distance of the test object but within 2 distance
				Position = new Vector2(0.8f, 3.7f)
			};

			// Initially this is outside the range, so we make sure it isn't detected as within
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);

			// Update the range so we can check its effect
			_proximity.Range = 2.0f;

			// After the change in range, the unit now does fall within our detection range
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.Within, _proximity.Status);
		}

		[Fact]
		public void DetectedCorrectlyAfterMovement()
		{
			var unit = new Unit(0)
			{
				// Should be not within 1 distance of the test object but within 2 distance
				Position = new Vector2(0.8f, 3.7f)
			};

			// Initially this is outside the range, so we make sure it isn't detected as within
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);

			// Change the position to now fall within the range
			unit.Position = new Vector2(2.4f, 3.4f);

			// The unit now does fall within our detection range
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.Within, _proximity.Status);

			// Change the position to now fall out of the range again
			unit.Position = new Vector2(2.3f, 4.5f);

			// The unit now doesn't fall within our detection range anymore
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);
		}
	}
}