using System.Collections.Generic;
using UnityEngine;


namespace Gamaga.Scripts.AstarPathfinding
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private Vector2 _gridWorldSize;
        [SerializeField] private float _nodeRadius = 0.15f;

        private readonly float _distanceBetweenNodes = 0.2f;
        private Node[,] _nodeArray;
        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;

        public Node[,] NodeArray { get => _nodeArray; set => _nodeArray = value; }

        private void Start()
        {
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
                    bool Wall = true;

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


        #region Gizmos
        private void OnDrawGizmos()
        {
            // We draw the Grid just for debug in here.
            Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

            if (_nodeArray != null)
            {
                foreach (Node n in _nodeArray)
                {
                    if (n.IsWall)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }
                    //Draw the node at the position of the node.
                    Gizmos.DrawCube(n.Position, Vector3.one * (_nodeDiameter - _distanceBetweenNodes));
                }
            }
        }
        #endregion Gizmos

    }

}
