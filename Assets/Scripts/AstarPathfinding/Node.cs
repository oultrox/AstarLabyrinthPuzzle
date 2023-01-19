using UnityEngine;

public class Node
{
    private int _posX;//X Position in the Node Array
    private int _posY;//Y Position in the Node Array
    private bool _isWall;//Tells the program if this node is being obstructed.
    private Vector3 _position;//The world position of the node.
    private Node _parentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
    private int _pathCost;//The cost of moving to the next square.
    private int _distanceCost;//The distance to the goal from this node.

    //Constructor
    public Node(bool isWall, Vector3 position, int posX, int posY)
    {
        IsWall = isWall;//Tells the program if this node is being obstructed.
        Position = position;//The world position of the node.
        PosX = posX;//X Position in the Node Array
        PosY = posY;//Y Position in the Node Array
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
