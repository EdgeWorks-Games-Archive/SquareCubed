using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

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

		public event EventHandler<ParentLinkEventArgs> ParentSet = (s, e) => { };
		public event EventHandler<ParentLinkEventArgs> ParentRemove = (s, e) => { };

		public sealed class ChildrenCollection : ICollection<TChild>
		{
			private readonly List<TChild> _children = new List<TChild>();
			private readonly Func<TChild, ParentLink<TParent, TChild>> _linkLocation;
			private readonly TParent _owner;

			public ChildrenCollection(TParent owner, Func<TChild, ParentLink<TParent, TChild>> linkLocation)
			{
				_owner = owner;
				_linkLocation = linkLocation;
			}

			public IEnumerator<TChild> GetEnumerator()
			{
				return _children.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Add(TChild child)
			{
				if (child == null)
					return;

				var link = _linkLocation(child);

				// If it already has a parent, remove it
				if (link._parent != null)
					link._collectionLocation(link._parent).Remove(child);

				// Add the child and set its parent
				_children.Add(child);
				link._parent = _owner;

				ChildAdd(this, new ParentLinkEventArgs(child, _owner));
				link.ParentSet(this, new ParentLinkEventArgs(child, _owner));
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains(TChild item)
			{
				throw new NotImplementedException();
			}

			public void CopyTo(TChild[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			public bool Remove(TChild child)
			{
				// TODO: Add custom exception for removing or adding during enumerating
				Debug.Assert(child != null);

				var link = _linkLocation(child);

				// Remove the child and reset its parent
				var ret = _children.Remove(child);
				link._parent = null;

				ChildRemove(this, new ParentLinkEventArgs(child, _owner));
				link.ParentRemove(this, new ParentLinkEventArgs(child, _owner));

				return ret;
			}

			public int Count
			{
				get { return _children.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public event EventHandler<ParentLinkEventArgs> ChildAdd = (s, e) => { };
			public event EventHandler<ParentLinkEventArgs> ChildRemove = (s, e) => { };
		}

		public sealed class ParentLinkEventArgs : EventArgs
		{
			public ParentLinkEventArgs(TChild child, TParent parent)
			{
				Child = child;
				Parent = parent;
			}

			public TChild Child { get; private set; }
			public TParent Parent { get; private set; }
		}
	}
}