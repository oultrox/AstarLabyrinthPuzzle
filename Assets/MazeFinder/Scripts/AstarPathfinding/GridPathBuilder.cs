using System.Collections.Generic;
using UnityEngine;


namespace Gamaga.Scripts.AstarPathfinding
{
    public class GridPathBuilder 
    {
        private readonly LayerMask _wallMask;
        private readonly Vector2 _gridWorldSize;
        private readonly float _nodeRadius;
        private readonly Transform _transform;
        private Node[,] _nodeArray;
        private readonly float _nodeDiameter;
        private readonly int _gridSizeX;
        private readonly int _gridSizeY;

        public Node[,] NodeArray { get => _nodeArray; set => _nodeArray = value; }
        public bool IsCreated { get; private set;}
            
        
        public GridPathBuilder(LayerMask wallMask, Vector2 gridWorldSize, float nodeRadius, Transform transform)
        {
            _wallMask = wallMask;
            _gridWorldSize = gridWorldSize;
            _nodeRadius = nodeRadius;
            _transform = transform;
            
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
            Vector3 bottomLeft = _transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    //Get the world coordinates of the bottom left of the graph
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
                    bool isWall = !Physics.CheckSphere(worldPoint, _nodeRadius, _wallMask);

                    _nodeArray[x, y] = new Node(isWall, worldPoint, x, y);
                }
            }

            IsCreated = true;
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
    }

}
