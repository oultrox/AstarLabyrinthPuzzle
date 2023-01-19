using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode _nodePrefab;
    [SerializeField] private Vector2Int _mazeSize;
    [SerializeField] private float _nodeSize;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _treasurePrefab;
    private GridPathFinder _pathFinder;
    private List<MazeNode> _nodes;
    private GameObject _player;
    private GameObject _treasure;
    private GameObject _parentMaze;
    private Vector3 _initialPos;
    private const string PARENT_NAME = "ParentMaze";

    private void Awake()
    {
        _pathFinder  = GetComponent<GridPathFinder>();
        _parentMaze = new GameObject();
        _parentMaze.transform.SetParent(this.transform);
        _parentMaze.name = PARENT_NAME;
    }

    public void StartMaze()
    {
        if(_nodes != null)
        {
            _nodes.Clear();
            ClearMaze();
        }

        GenerateMaze(_mazeSize);
        InitializePlayer();
        InitializeTreasure();
        StartCoroutine(PathfindingInitialization());
    }

    private IEnumerator PathfindingInitialization()
    {
        _pathFinder.HidePath();
        yield return new WaitForSeconds(0.2f);
        _pathFinder.Grid.CreateGrid();
        _pathFinder.FindPath(_initialPos, _treasure.transform.position);
    }

    private void ClearMaze()
    {
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
        _pathFinder.Grid.CreateGrid();
    }

    private void InitializePlayer()
    {
        var nodePosition = _nodes[0].transform.position;
        nodePosition.y = 0.02f;
        _initialPos = nodePosition;
        if (_player == null)
        {
            _player = Instantiate(_playerPrefab, nodePosition, Quaternion.identity);
        }
        else
        {
            CharacterController charController = _player.GetComponent<CharacterController>();
            charController.enabled = false;
            charController.transform.position = nodePosition;
            charController.enabled = true;
        }
    }

    private void InitializeTreasure()
    {
        int lastIndex = _nodes.Count - 1;
        var nodePosition = _nodes[lastIndex].transform.position;
        nodePosition.y = 0.33f;
        if(_treasure == null)
        {
            _treasure = Instantiate(_treasurePrefab, nodePosition, Quaternion.identity);
        }
        else
        {
            _treasure.transform.position = nodePosition;
            _treasure.SetActive(true);
        }
    }

}
