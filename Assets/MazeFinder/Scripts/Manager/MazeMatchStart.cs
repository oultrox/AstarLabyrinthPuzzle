using AstarPathfinding;
using Gamaga.Scripts.AstarPathfinding;
using Gamaga.Scripts.MazeGeneration;
using MazeFinder.Scripts.Events;
using MazeGeneration;
using Player;
using SimpleBus;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    /// <summary>
    /// Single Entry Point layer of the maze test, Manual injector.
    /// </summary>
    public class MazeMatchStart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject pathDrawerPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject treasurePrefab;
        
        [Header("Grid Data")]
        [SerializeField] private MazeNode mazeNode;
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private Vector2Int _gridWorldSize;
        
        [Header("GUI")]
        [SerializeField] private Button generateButton;
        [SerializeField] private Button showPathButton;
        
        private const float NODE_RADIUS = 0.15f;
        private IPathFinder _pathFinder;
        private IEntitySpawner _entitySpawner;
        private IMazeGenerator _mazeGenerator;
        private readonly MapGeneratedEvent _mapGenerated = new();
        private readonly ShowMazePathEvent _showMazePath = new();
        
        
        void Start()
        {
            SetBehavioralComponents();
            InjectListeners();
        }

        void SetBehavioralComponents()
        {
            var grid = new GridPathBuilder(_wallMask, _gridWorldSize, NODE_RADIUS, transform);
            var pathDrawer = Instantiate(pathDrawerPrefab).GetComponent<PathDrawer>();
            
            _entitySpawner = new EntitySpawner(playerPrefab, treasurePrefab);
            _pathFinder = new GridPathFinder(grid, pathDrawer, _entitySpawner);
            _mazeGenerator = new MazeGenerator(_pathFinder, _entitySpawner, _gridWorldSize, mazeNode);
        }

        void InjectListeners()
        {
            generateButton.onClick.AddListener(GenerateMap);
            showPathButton.onClick.AddListener(ShowPath);
        }

         void GenerateMap()
        {
            EventBus<MapGeneratedEvent>.Raise(_mapGenerated);
        }

        void ShowPath()
        {
            EventBus<ShowMazePathEvent>.Raise(_showMazePath);
        }
    }
}
