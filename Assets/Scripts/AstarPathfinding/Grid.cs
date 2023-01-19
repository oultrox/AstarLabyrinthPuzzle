using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _distanceBetweenNodes;
    [SerializeField] private GameObject pathRender;
    private float _nodeRadius = 0.2f;
    private Node[,] _nodeArray;
    private List<Node> _finalPath;

    private Vector3 _pathPosition;
    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY;
    private GameObject _gridDebuggerParent;

    public List<Node> FinalPath { get => _finalPath; set => _finalPath = value; }

    private void Start()
    {
        _gridDebuggerParent = new GameObject();
        _gridDebuggerParent.transform.SetParent(this.transform);
        _gridDebuggerParent.name = "PathDebugger";
        _gridDebuggerParent.SetActive(false);
        
        //Double the radius to get diameter
        _nodeDiameter = _nodeRadius * 2;

        //Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
    }

    //Declare the array of node and create the grid from the array.
    public void CreateGrid()
    {
        _nodeArray = new Node[_gridSizeX, _gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                //Get the world co ordinates of the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
                bool Wall = true;//Make the node a wall

                if (Physics.CheckSphere(worldPoint, _nodeRadius, _wallMask))
                {
                    Wall = false;
                }

                _nodeArray[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighborNode(Node neighborNode)
    {
        List<Node> NeighborList = new List<Node>();
        int icheckX;
        int icheckY;

        //Check the right side of the current node.
        icheckX = neighborNode.PosX + 1;
        icheckY = neighborNode.PosY;
        if (icheckX >= 0 && icheckX < _gridSizeX)
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);
            }
        }
        //Check the Left side of the current node.
        icheckX = neighborNode.PosX - 1;
        icheckY = neighborNode.PosY;
        if (icheckX >= 0 && icheckX < _gridSizeX)
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);
            }
        }
        //Check the Top side of the current node.
        icheckX = neighborNode.PosX;
        icheckY = neighborNode.PosY + 1;
        if (icheckX >= 0 && icheckX < _gridSizeX)
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);
            }
        }
        //Check the Bottom side of the current node.
        icheckX = neighborNode.PosX;
        icheckY = neighborNode.PosY - 1;
        if (icheckX >= 0 && icheckX < _gridSizeX)
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);
            }
        }

        return NeighborList;
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float ixPos = ((worldPos.x + _gridWorldSize.x / 2) / _gridWorldSize.x);
        float iyPos = ((worldPos.z + _gridWorldSize.y / 2) / _gridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((_gridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((_gridSizeY - 1) * iyPos);

        return _nodeArray[ix, iy];
    }


    //TODO: Move this to GridPathFinder.
    public void DrawPath()
    {
        if (_nodeArray != null)//If the grid is not empty
        {
            for (var i = _gridDebuggerParent.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
            }

            foreach (Node n in _nodeArray)//Loop through every node in the grid
            {
                //If the final path is not empty
                if (_finalPath == null)
                {
                    continue;
                }
                
                if (_finalPath.Contains(n))//If the current node is in the final path
                {
                    _pathPosition = n.Position;
                    _pathPosition.y = 0.02f;
                    Instantiate(pathRender, _pathPosition, Quaternion.identity, _gridDebuggerParent.transform);
                }
            }
        }
    }

    public void ShowPath()
    {
        bool isActive = !_gridDebuggerParent.activeSelf;
        _gridDebuggerParent.SetActive(isActive);
    }

    public void HidePath()
    {
        _gridDebuggerParent.SetActive(false);
    }

    #region Gizmos
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (_nodeArray != null)//If the grid is not empty
        {
            foreach (Node n in _nodeArray)//Loop through every node in the grid
            {
                if (n.IsWall)//If the current node is a wall node
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
                }


                if (_finalPath != null)//If the final path is not empty
                {
                    if (_finalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }

                }


                Gizmos.DrawCube(n.Position, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
    #endregion Gizmos

}
