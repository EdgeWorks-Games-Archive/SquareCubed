using System;
using Lidgren.Network;

namespace SquareCubed.Network
{
	public class ConnectApprovalEventArgs : EventArgs
	{
		private bool _deny;

		public ConnectApprovalEventArgs(string name, NetConnection connection)
		{
			Name = name;
			Connection = connection;
			_deny = false;
		}

		public string Name { get; private set; }
		public NetConnection Connection { get; private set; }

		public bool Deny
		{
			get { return _deny; }
			set
			{
				if (!value)
					throw new InvalidOperationException("Can only deny, not overwrite denies.");

				_deny = true;
			}
		}
	}
}