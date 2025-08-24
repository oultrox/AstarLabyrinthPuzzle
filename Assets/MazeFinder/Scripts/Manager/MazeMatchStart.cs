using AstarPathfinding;
using Gamaga.Scripts.AstarPathfinding;
using MazeFinder.Scripts.Events;
using MazeGeneration;
using Player;
using SimpleBus;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    /// <summary>
    /// Highest layer of the maze test, Manual injector.
    /// </summary>
    public class MazeMatchStart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private EntitySpawner entitySpawner;
        [SerializeField] private GameObject pathFinderObj;
        [SerializeField] private GameObject pathDrawerPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject treasurePrefab;
       
        
        [Header("Map References")]
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private Vector2Int _mazeSize;
        [SerializeField] private float _nodeSize;
        
        [Header("Grid Data")]
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private Vector2 _gridWorldSize;
        [SerializeField] private float _nodeRadius = 0.15f;
        
        [Header("GUI")]
        [SerializeField] private Button generateButton;
        [SerializeField] private Button showPathButton;
        
        private IPathFinder _pathFinder;
        private readonly MapGeneratedEvent _mapGenerated = new();
        private readonly ShowMazePathEvent _showMazePath = new();
        
        
        void Start()
        {
            SetBehavioralComponents();
            InjectListeners();
        }

        void SetBehavioralComponents()
        {
            var grid = new GridPathBuilder(_wallMask, _gridWorldSize, _nodeRadius, transform);
            var pathDrawer = Instantiate(pathDrawerPrefab).GetComponent<PathDrawer>();
            
            _pathFinder = new GridPathFinder(grid, pathDrawer, entitySpawner);
            mazeGenerator.Initialize(_pathFinder, entitySpawner);
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
