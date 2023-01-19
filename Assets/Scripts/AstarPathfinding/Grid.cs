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
    private Node[,] _nodeArray;//The array of nodes that the A Star algorithm uses.
    private List<Node> _finalPath;//The completed path that the red line will be drawn along

    private Vector3 _pathPosition;
    private float _nodeDiameter;//Twice the amount of the radius (Set in the start function)
    private int _gridSizeX, _gridSizeY;//Size of the Grid in Array units.
    private GameObject _gridDebuggerParent;

    public List<Node> FinalPath { get => _finalPath; set => _finalPath = value; }

    private void Start()//Ran once the program starts
    {
        _gridDebuggerParent = Instantiate(new GameObject("DebuggerPath"), Vector3.zero, Quaternion.identity, transform);
        _gridDebuggerParent.SetActive(false);
        _nodeDiameter = _nodeRadius * 2;//Double the radius to get diameter
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
    }

    public void CreateGrid()
    {
        _nodeArray = new Node[_gridSizeX, _gridSizeY];//Declare the array of nodes.
        Vector3 bottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < _gridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < _gridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = true;//Make the node a wall

                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a _wallMask,
                //The if statement will return false.
                if (Physics.CheckSphere(worldPoint, _nodeRadius, _wallMask))
                {
                    Wall = false;//Object is not a wall
                }

                _nodeArray[x, y] = new Node(Wall, worldPoint, x, y);//Create a new node in the array.
            }
        }
    }

    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < _gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < _gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < _gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < _gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < _gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(_nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + _gridWorldSize.x / 2) / _gridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + _gridWorldSize.y / 2) / _gridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((_gridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((_gridSizeY - 1) * iyPos);

        return _nodeArray[ix, iy];
    }

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
                    _pathPosition = n.vPosition;
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
                if (n.bIsWall)//If the current node is a wall node
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


                Gizmos.DrawCube(n.vPosition, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
    #endregion Gizmos

}
