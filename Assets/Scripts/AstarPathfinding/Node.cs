using UnityEngine;


namespace Gamaga.Scripts.AstarPathfinding
{
    //Node class for the AStar algoritm, will also store what node it previously came from so it cn trace the shortest path.
    public class Node
    {
        private int _posX;
        private int _posY;
        private bool _isWall;
        private Vector3 _position;
        private Node _parentNode;
        private int _pathCost;
        private int _distanceCost;

        //Constructor
        public Node(bool isWall, Vector3 position, int posX, int posY)
        {
            IsWall = isWall;
            Position = position;
            PosX = posX;
            PosY = posY;
        }

        #region properties
        public int FCost { get { return PathCost + DistanceCost; } }
        public int PosX { get => _posX; set => _posX = value; }
        public int PosY { get => _posY; set => _posY = value; }
        public bool IsWall { get => _isWall; set => _isWall = value; }
        public Node ParentNode { get => _parentNode; set => _parentNode = value; }
        public int PathCost { get => _pathCost; set => _pathCost = value; }
        public int DistanceCost { get => _distanceCost; set => _distanceCost = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        #endregion
    }
}
