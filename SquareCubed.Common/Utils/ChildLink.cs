using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SquareCubed.Common.Utils
{
	public sealed class ParentLink<TParent, TChild> where TParent : class where TChild : class
	{
		private readonly Func<TParent, ChildrenCollection> _collectionLocation;
		private readonly TChild _owner;
		private TParent _parent;

		public ParentLink(TChild owner, Func<TParent, ChildrenCollection> collectionLocation)
		{
			_owner = owner;
			_collectionLocation = collectionLocation;
		}

		public TParent Property
		{
			get { return _parent; }
			set
			{
				// If already this, don't do anything
				if (_parent == value) return;

				// If new parent isn't nothing
				if (value != null)
				{
					// Tell the new parent we need to be added
					// Adding will remove the old parent and set the new one
					_collectionLocation(value).Add(_owner);
				}
				else
				{
					// Tell the old parent we need to be removed
					_collectionLocation(_parent).Remove(_owner);
				}
			}
		}

		public sealed class ChildrenCollection
		{
			private readonly List<TChild> _children = new List<TChild>();
			private readonly Func<TChild, ParentLink<TParent, TChild>> _linkLocation;
			private readonly TParent _owner;

			public ChildrenCollection(TParent owner, Func<TChild, ParentLink<TParent, TChild>> linkLocation)
			{
				_owner = owner;
				_linkLocation = linkLocation;
			}

			public void Add(TChild child)
			{
				Contract.Requires<ArgumentNullException>(child != null);

				var link = _linkLocation(child);

				// If it already has a parent, remove it
				if (link._parent != null)
					link._collectionLocation(link._parent).Remove(child);

				// Add the child and set its parent
				_children.Add(child);
				link._parent = _owner;
			}

			public void Remove(TChild child)
			{
				Contract.Requires<ArgumentNullException>(child != null);

				// Remove the child and reset its parent
				_children.Remove(child);
				_linkLocation(child)._parent = null;
			}
		}
	}
}