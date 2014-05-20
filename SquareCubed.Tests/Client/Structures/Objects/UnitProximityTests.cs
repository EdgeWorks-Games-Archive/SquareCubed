using Moq;
using OpenTK;
using SquareCubed.Client.Structures;
using SquareCubed.Client.Structures.Objects;
using SquareCubed.Client.Units;
using Xunit;

namespace SquareCubed.Tests.Client.Structures.Objects
{
	public class UnitProximityTests
	{
		private readonly UnitProximityHelper _proximity;
		private readonly ClientStructure _structure = new ClientStructure();

		public UnitProximityTests()
		{
			var obj = Mock.Of<ClientObjectBase>(o =>
				o.Position == new Vector2(2.0f, 3.5f) &&
				o.Parent == _structure);
			_proximity = new UnitProximityHelper(obj);
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
				Position = new Vector2(1.8f, 3.6f),
				Structure = _structure
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
				Position = new Vector2(1.0f, 3.7f),
				Structure = _structure
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
				Position = new Vector2(0.8f, 3.7f),
				Structure = _structure
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
				Position = new Vector2(0.8f, 3.7f),
				Structure = _structure
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

		[Fact]
		public void EventsFiredCorrectly()
		{
			var unit = new Unit(0)
			{
				// Should be not within 1 distance of the test object but within 2 distance
				Position = new Vector2(0.8f, 3.7f),
				Structure = _structure
			};

			// Set up the Event
			var evtProximityStatus = ProximityStatus.NotWithin;
			var evtTriggerCount = 0;
			_proximity.Change += (s, e) =>
			{
				evtTriggerCount++;
				evtProximityStatus = e.NewStatus;
			};

			// Initially this is outside the range, so we make sure it isn't detected as within
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, evtProximityStatus);
			Assert.Equal(0, evtTriggerCount);

			// Change the position to now fall within the range
			unit.Position = new Vector2(2.4f, 3.4f);

			// The unit now does fall within our detection range
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.Within, evtProximityStatus);
			Assert.Equal(1, evtTriggerCount);

			// Change the position to now fall out of the range again
			unit.Position = new Vector2(2.3f, 4.5f);

			// The unit now doesn't fall within our detection range anymore
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, evtProximityStatus);
			Assert.Equal(2, evtTriggerCount);
		}

		[Fact]
		public void ConstructorChangesRange()
		{
			var helper = new UnitProximityHelper(null, 0.5f);
			Assert.Equal(0.5f, helper.Range);
		}

		[Fact]
		public void DoesNotDetectAcrossStructures()
		{
			var unit = new Unit(0)
			{
				// Should be within 1 distance of the test object
				Position = new Vector2(1.8f, 3.6f),
				Structure = new ClientStructure()
			};
			_proximity.Update(unit);
			Assert.Equal(ProximityStatus.NotWithin, _proximity.Status);
		}
	}
}