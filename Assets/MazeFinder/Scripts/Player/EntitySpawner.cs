using AstarPathfinding;
using MazeGeneration;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Just spawns the player and the treasure based on given positions.
    /// </summary>
    public class EntitySpawner : IEntitySpawner
    {
        private readonly GameObject _playerPrefab;
        private readonly GameObject _treasurePrefab;
        private GameObject _player;
        private GameObject _treasure;
        
        Vector3 InitialPlayerPos { get; set; }
        public Vector3 PlayerPosition => _player == null ? InitialPlayerPos : _player.transform.position;
        public Vector3 TreasurePosition => _treasure == null ? InitialPlayerPos : _treasure.transform.position;
        
        
        public EntitySpawner(GameObject playerPrefab, GameObject treasurePrefab)
        {
            _playerPrefab = playerPrefab;
            _treasurePrefab = treasurePrefab;
        }
        
        public void SpawnEntities(IMazeGenerator mazeGenerator)
        {
            SpawnPlayer(mazeGenerator.GetStartPosition());
            SpawnTreasure(mazeGenerator.GetEndGoalPosition());
        }

        void SpawnPlayer(Vector3 position)
        {
            var nodePosition = position;
            nodePosition.y = 0.02f;
            InitialPlayerPos = nodePosition;
            if (_player == null)
            {
                _player = Object.Instantiate(_playerPrefab, nodePosition, Quaternion.identity);
            }
            else
            {
                CharacterController charController = _player.GetComponent<CharacterController>();
                charController.enabled = false;
                charController.transform.position = nodePosition;
                charController.enabled = true;
            }
        }

        void SpawnTreasure(Vector3 position)
        {
            var nodePosition = position;
            nodePosition.y = 0.33f;
            if (_treasure == null)
            {
                _treasure = Object. Instantiate(_treasurePrefab, nodePosition, Quaternion.identity);
            }
            else
            {
                _treasure.transform.position = nodePosition;
                _treasure.SetActive(true);
            }
        }
    }
}
