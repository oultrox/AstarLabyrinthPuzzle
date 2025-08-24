using AstarPathfinding;
using Gamaga.Scripts.MazeGeneration;
using MazeFinder.Scripts.Events;
using MazeGeneration;
using Player;
using SimpleBus;
using UnityEngine;
using UnityEngine.UI;
using Grid = Gamaga.Scripts.AstarPathfinding.Grid;

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
        [SerializeField] private MazeNode _nodePrefab;
        
        [Header("GUI")]
        [SerializeField] private Button generateButton;
        [SerializeField] private Button showPathButton;
        
        private IPathGenerator _pathGenerator;
        private readonly MapGeneratedEvent _mapGenerated = new();
        private readonly ShowMazePathEvent _showMazePath = new();
        
        
        void Start()
        {
            SetBehavioralComponents();
            InjectListeners();
        }

        private void SetBehavioralComponents()
        {
            var grid = pathFinderObj.GetComponent<Grid>(); 
            var pathDrawer = Instantiate(pathDrawerPrefab).GetComponent<PathDrawer>();
            _pathGenerator = new GridPathFinder(grid, pathDrawer, entitySpawner);
            mazeGenerator.Initialize(_pathGenerator, entitySpawner);
        }

        private void InjectListeners()
        {
            generateButton.onClick.AddListener(GenerateMap);
            showPathButton.onClick.AddListener(ShowPath);
        }

        private  void GenerateMap()
        {
            EventBus<MapGeneratedEvent>.Raise(_mapGenerated);
        }

        private void ShowPath()
        {
            EventBus<ShowMazePathEvent>.Raise(_showMazePath);
        }
    }
}
