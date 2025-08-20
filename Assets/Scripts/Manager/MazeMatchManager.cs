using System.Threading.Tasks;
using AstarPathfinding;
using Gamaga.Scripts.AstarPathfinding;
using Gamaga.Scripts.MazeGeneration;
using MazeGeneration;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Grid = Gamaga.Scripts.AstarPathfinding.Grid;

namespace Manager
{
    /// <summary>
    /// Highest layer of the maze test, orchestrator.
    /// </summary>
    public class MazeMatchManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private EntitySpawner entitySpawner;
        [SerializeField] private GameObject pathFinderObj;
        [SerializeField] private GameObject pathDrawerPrefab;
        [Header("GUI")]
        [SerializeField] private Button generateButton;
        [SerializeField] private Button showPathButton;
        [Header("Maze Match Settings")]
 
        private IPathGenerator _pathGenerator;
        private IEntitySpawner _spawner;
        private IMazeGenerator _mazeGenerator;
        
        
        void Start()
        {
            SetBehavioralComponents();
            InjectListeners();
        }

        private void SetBehavioralComponents()
        {
            var grid = pathFinderObj.GetComponent<Grid>(); 
            var pathDrawer = Instantiate(pathDrawerPrefab).GetComponent<PathDrawer>();
            _mazeGenerator = mazeGenerator;
            _pathGenerator = new GridPathFinder(grid, pathDrawer);
            _spawner = entitySpawner;
        }

        private void InjectListeners()
        {
            generateButton.onClick.AddListener(async () => await InitMazeWithEntitiesAsync());
            showPathButton.onClick.AddListener(async () => await InitFindPathAsync());
        }

        private async Task InitMazeWithEntitiesAsync()
        {
            if (_pathGenerator.IsGeneratingPath) return;

            mazeGenerator.InitializeMaze();
            await _pathGenerator.InitializeGridAsync();
            _spawner.SpawnEntities(_mazeGenerator);
        }

        private async Task InitFindPathAsync()
        {
            await _pathGenerator.FindSolutionAsync(_spawner);
        }
    }
}
