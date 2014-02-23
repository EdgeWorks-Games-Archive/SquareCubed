namespace SquareCubed.Client.Player
{
	public class Player
	{
		private const float Speed = 2;
		private readonly Client _client;
		private readonly PlayerNetwork _network;
		private PlayerUnit _playerUnit;

		public Player(Client client)
		{
			_client = client;
			_network = new PlayerNetwork(_client, this);
		}

		public void OnPlayerData(uint id)
		{
			var unit = _client.Units.GetAndRemove(id);
			_playerUnit = new PlayerUnit(unit);
			_client.Units.Add(_playerUnit);
		}

		public void Update(float delta)
		{
			if (_playerUnit == null) return;

			_playerUnit.Position += _client.Input.Axes*delta*Speed;
			_network.SendPlayerPhysics(_playerUnit);
		}
	}
}