using SquareCubed.Common.Utils;
using Xunit;

namespace SquareCubed.Tests.Common.Utils
{
	using ParentLink = ParentLink<ParentLinkTests.ChildObj.ParentObj, ParentLinkTests.ChildObj>;

	public class ParentLinkTests
	{
		private readonly ChildObj _childObj = new ChildObj();
		private readonly ChildObj.ParentObj _parentObj = new ChildObj.ParentObj();

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
			var set = false;
			_parentObj.Children.ChildAdd += (s, e) => set = true;
			_parentObj.Children.Add(_childObj);
			Assert.True(set);
		}

		[Fact]
		public void RemovingTriggersEvents()
		{
			var set = false;
			_parentObj.Children.ChildAdd += (s, e) => set = true;
			_parentObj.Children.Add(_childObj);
			Assert.True(set);
		}

		public sealed class ChildObj
		{
			private readonly ParentLink _parent;

			public ChildObj()
			{
				_parent = new ParentLink(this, p => p.Children);
			}

			public ParentObj Parent
			{
				get { return _parent.Property; }
				set { _parent.Property = value; }
			}

			public sealed class ParentObj
			{
				public ParentObj()
				{
					Children = new ParentLink.ChildrenCollection(this, c => c._parent);
				}

				public ParentLink.ChildrenCollection Children { get; private set; }
			}
		}
	}
}