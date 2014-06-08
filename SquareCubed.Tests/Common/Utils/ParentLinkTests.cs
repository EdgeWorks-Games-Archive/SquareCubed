using SquareCubed.Common.Utils;
using Xunit;

namespace SquareCubed.Tests.Common.Utils
{
	using ParentLink = ParentLink<ParentLinkTests.ParentObj, ParentLinkTests.ChildObj>;

	public class ParentLinkTests
	{
		private readonly ChildObj _childObj = new ChildObj();
		private readonly ParentObj _parentObj = new ParentObj();

		[Fact]
		public void SettingParentParents()
		{
			_childObj.Parent = _parentObj;
			Assert.Same(_parentObj, _childObj.Parent);
			Assert.Contains(_childObj, _parentObj.Children);
		}

		[Fact]
		public void SettingParentToNullRemoves()
		{
			_childObj.Parent = _parentObj;
			_childObj.Parent = null;
			Assert.Same(null, _childObj.Parent);
			Assert.NotSame(_parentObj, _childObj.Parent);
			Assert.DoesNotContain(_childObj, _parentObj.Children);
		}

		[Fact]
		public void AddingToParentParents()
		{
			_parentObj.Children.Add(_childObj);
			Assert.Same(_parentObj, _childObj.Parent);
			Assert.Contains(_childObj, _parentObj.Children);
		}

		[Fact]
		public void RemovingFromParentRemoves()
		{
			_parentObj.Children.Add(_childObj);
			_parentObj.Children.Remove(_childObj);
			Assert.Same(null, _childObj.Parent);
			Assert.NotSame(_parentObj, _childObj.Parent);
			Assert.DoesNotContain(_childObj, _parentObj.Children);
		}

		[Fact]
		public void AddingTriggersEvents()
		{
			var setParent = false;
			_parentObj.Children.ChildAdd += (s, e) => setParent = true;
			var setChild = false;
			_childObj.ParentLink.ParentSet += (s, e) => setChild = true;

			_parentObj.Children.Add(_childObj);

			Assert.True(setParent, "setParent was not set to true!");
			Assert.True(setChild, "setChild was not set to true!");
		}

		[Fact]
		public void RemovingTriggersEvents()
		{
			var setParent = false;
			_parentObj.Children.ChildRemove += (s, e) => setParent = true;
			var setChild = false;
			_childObj.ParentLink.ParentRemove += (s, e) => setChild = true;

			_parentObj.Children.Add(_childObj);
			_parentObj.Children.Remove(_childObj);

			Assert.True(setParent, "setParent was not set to true!");
			Assert.True(setChild, "setChild was not set to true!");
		}

		public sealed class ChildObj
		{
			public ChildObj()
			{
				ParentLink = new ParentLink(this, p => p.Children);
			}

			public ParentLink ParentLink { get; private set; }
			public ParentObj Parent
			{
				get { return ParentLink.Property; }
				set { ParentLink.Property = value; }
			}
		}

		public sealed class ParentObj
		{
			public ParentObj()
			{
				Children = new ParentLink.ChildrenCollection(this, c => c.ParentLink);
			}

			public ParentLink.ChildrenCollection Children { get; private set; }
		}
	}
}