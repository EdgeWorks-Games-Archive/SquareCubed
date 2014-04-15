using System.Collections.Generic;

namespace SquareCubed.Common.Utils
{
	/// <summary>
	///     Functions just like a regular dictionary but maintains
	///     an internal counter to automatically assign keys.
	/// </summary>
	/// <typeparam name="TValue">Value to map in dictionary.</typeparam>
	public class AutoDictionary<TValue> : Dictionary<int, TValue>
	{
		private int _nextKey;

		public int Add(TValue value)
		{
			// Add the value with the current next key
			base.Add(_nextKey, value);

			// Return key and increment it
			return _nextKey++;
		}

		public new void Add(int key, TValue value)
		{
			base.Add(key, value);

			// Increment next key if needed
			if (key >= _nextKey)
				_nextKey = key + 1;
		}
	}
}