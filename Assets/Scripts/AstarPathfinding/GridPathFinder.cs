using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridPathFinder : MonoBehaviour
{
    [SerializeField] private GameObject pathRender;
    private Grid _grid;
    private List<Node> _finalPath;
    private Vector3 _pathPosition;
    private GameObject _gridDebuggerParent;

    public Grid Grid { get => _grid; set => _grid = value; }

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _gridDebuggerParent = new GameObject();
        _gridDebuggerParent.transform.SetParent(this.transform);
        _gridDebuggerParent.name = "PathDebugger";
        _gridDebuggerParent.SetActive(false);
    }

    public void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = _grid.NodeFromWorldPoint(a_StartPos);
        Node TargetNode = _grid.NodeFromWorldPoint(a_TargetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].DistanceCost < CurrentNode.DistanceCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighborNode in _grid.GetNeighborNode(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.PathCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.PathCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.PathCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.DistanceCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }
        }
        DrawPath(_grid.NodeArray);
    }

    private void DrawPath(Node[,] nodeArray)
    {
        if (nodeArray != null)
        {
            for (var i = _gridDebuggerParent.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
            }

            foreach (Node n in nodeArray)
            {
                if (_finalPath == null)
                {
                    continue;
                }

                if (_finalPath.Contains(n))
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

    void GetFinalPath(Node startingNode, Node endNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = endNode;//Node to store the current node being checked

        while (CurrentNode != startingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        _finalPath = FinalPath;//Set the final path

    }

    int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.PosX - nodeB.PosX);//x1-x2
        int iy = Mathf.Abs(nodeA.PosY - nodeB.PosY);//y1-y2

        return ix + iy;//Return the sum
    }
}
