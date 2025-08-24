using System.Collections.Generic;
using AstarPathfinding;
using Gamaga.Scripts.MazeGeneration;
using MazeFinder.Scripts.Events;
using SimpleBus;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MazeGeneration
{
    /// <summary>
    /// In charge of generating the maze with a node-based system.
    /// </summary>
    public class MazeGenerator : MonoBehaviour, IMazeGenerator
    {
        [SerializeField] private MazeNode _nodePrefab;
        [FormerlySerializedAs("_nodeSize")] [SerializeField] private float _nodeRadius;
        private Vector2Int _mazeSize;
        private List<MazeNode> _nodes;
        private GameObject _parentMaze;
        private const string PARENT_NAME = "ParentMaze";
        private EventBinder<MapGeneratedEvent> _mapGeneratedBinder;
        IPathFinder _pathFinder;
        IEntitySpawner _spawner;
        
        
        void Awake()
        {
            _parentMaze = new GameObject();
            _parentMaze.transform.SetParent(transform);
            _parentMaze.name = PARENT_NAME;
        }

        void OnEnable()
        {
            _mapGeneratedBinder = new EventBinder<MapGeneratedEvent>(Generate);
            EventBus<MapGeneratedEvent>.Register(_mapGeneratedBinder);
        }

        void OnDisable()
        {
            EventBus<MapGeneratedEvent>.Deregister(_mapGeneratedBinder);
        }
        
        public void Initialize(IPathFinder pathFinder, IEntitySpawner entitySpawner, Vector2Int gridWorldSize, float nodeRadius)
        {
            _pathFinder = pathFinder;
            _spawner = entitySpawner;
            _mazeSize = gridWorldSize;
            _nodeRadius = nodeRadius;
        }

        void Generate()
        {
            if (_pathFinder.IsGeneratingPath) return;
            InitializeMaze();
            _pathFinder.InitializeGridAsync();
            _spawner.SpawnEntities(this);
        }

        

        public Vector3 GetStartPosition()
        {
            return GetFirstNodePosition();
        }

        public Vector3 GetEndGoalPosition()
        {
            return GetLastNodePosition();
        }
        
        void InitializeMaze()
        {
            if (_nodes != null)
            {
                ClearMaze();
            }
            GenerateMaze(_mazeSize);
        }
        
        void ClearMaze()
        {
            _nodes.Clear();
            for (var i = _parentMaze.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_parentMaze.transform.GetChild(i).gameObject);
            }
        }
        
        void GenerateMaze(Vector2Int size)
        {
            _nodes = new List<MazeNode>();

            // Create _nodes
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                    MazeNode newNode = Instantiate(_nodePrefab, nodePos, Quaternion.identity, _parentMaze.transform);
                    _nodes.Add(newNode);
                }
            }

            List<MazeNode> currentPath = new List<MazeNode>();
            List<MazeNode> completedNodes = new List<MazeNode>();

            // Choose starting node
            currentPath.Add(_nodes[Random.Range(0, _nodes.Count)]);

            while (completedNodes.Count < _nodes.Count)
            {
                // Check _nodes next to the current node
                List<int> possibleNextNodes = new List<int>();
                List<int> possibleDirections = new List<int>();

                int currentNodeIndex = _nodes.IndexOf(currentPath[currentPath.Count - 1]);
                int currentNodeX = currentNodeIndex / size.y;
                int currentNodeY = currentNodeIndex % size.y;

                if (currentNodeX < size.x - 1)
                {
                    // Check node to the right of the current node
                    if (!completedNodes.Contains(_nodes[currentNodeIndex + size.y]) &&
                        !currentPath.Contains(_nodes[currentNodeIndex + size.y]))
                    {
                        possibleDirections.Add(1);
                        possibleNextNodes.Add(currentNodeIndex + size.y);
                    }
                }
                if (currentNodeX > 0)
                {
                    // Check node to the left of the current node
                    if (!completedNodes.Contains(_nodes[currentNodeIndex - size.y]) &&
                        !currentPath.Contains(_nodes[currentNodeIndex - size.y]))
                    {
                        possibleDirections.Add(2);
                        possibleNextNodes.Add(currentNodeIndex - size.y);
                    }
                }
                if (currentNodeY < size.y - 1)
                {
                    // Check node above the current node
                    if (!completedNodes.Contains(_nodes[currentNodeIndex + 1]) &&
                        !currentPath.Contains(_nodes[currentNodeIndex + 1]))
                    {
                        possibleDirections.Add(3);
                        possibleNextNodes.Add(currentNodeIndex + 1);
                    }
                }
                if (currentNodeY > 0)
                {
                    // Check node below the current node
                    if (!completedNodes.Contains(_nodes[currentNodeIndex - 1]) &&
                        !currentPath.Contains(_nodes[currentNodeIndex - 1]))
                    {
                        possibleDirections.Add(4);
                        possibleNextNodes.Add(currentNodeIndex - 1);
                    }
                }

                // Choose next node
                if (possibleDirections.Count > 0)
                {
                    int chosenDirection = Random.Range(0, possibleDirections.Count);
                    MazeNode chosenNode = _nodes[possibleNextNodes[chosenDirection]];

                    switch (possibleDirections[chosenDirection])
                    {
                        case 1:
                            chosenNode.RemoveWall(1);
                            currentPath[currentPath.Count - 1].RemoveWall(0);
                            break;
                        case 2:
                            chosenNode.RemoveWall(0);
                            currentPath[currentPath.Count - 1].RemoveWall(1);
                            break;
                        case 3:
                            chosenNode.RemoveWall(3);
                            currentPath[currentPath.Count - 1].RemoveWall(2);
                            break;
                        case 4:
                            chosenNode.RemoveWall(2);
                            currentPath[currentPath.Count - 1].RemoveWall(3);
                            break;
                    }

                    currentPath.Add(chosenNode);
                }
                else
                {
                    completedNodes.Add(currentPath[currentPath.Count - 1]);

                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }
        
        Vector3 GetFirstNodePosition()
        {
            return _nodes[0].transform.position;
        }
        
        Vector3 GetLastNodePosition()
        {
            if (_nodes.Count == 0) return Vector3.zero;
            return _nodes[^1].transform.position;
        }
    }
}

