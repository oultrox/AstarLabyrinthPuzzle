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

    #region Properties
    public Grid Grid { get => _grid; set => _grid = value; }
    #endregion

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

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].DistanceCost < CurrentNode.DistanceCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            //Loop through each neighbor of the current node
            foreach (Node NeighborNode in _grid.GetNeighborNode(CurrentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                //Get the F cost of that neighbor
                int MoveCost = CurrentNode.PathCost + GetManhattenDistance(CurrentNode, NeighborNode);

                //If the f cost is greater than the g cost or it is not in the open list
                if (MoveCost < NeighborNode.PathCost || !OpenList.Contains(NeighborNode))
                {
                    //Set the g cost to the f cost
                    NeighborNode.PathCost = MoveCost;
                    NeighborNode.DistanceCost = GetManhattenDistance(NeighborNode, TargetNode);
                    
                    //Set the parent of the node for retracing steps
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
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
            ClearPath();
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


    internal IEnumerator InitializeGrid()
    {
        HidePath();
        yield return new WaitForSeconds(0f);
        Grid.CreateGrid();
    }

    internal IEnumerator FindSolution(Vector3 initialPos, Vector3 targetPos)
    {
        HidePath();
        ClearPath();
         
        yield return new WaitForSeconds(0.1f);
        FindPath(initialPos, targetPos);
        yield return new WaitForSeconds(0f);
        ShowPath();
    }

    public void ShowPath()
    {
        _gridDebuggerParent.SetActive(true);
    }

    private void ClearPath()
    {
        for (var i = _gridDebuggerParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
        }
    }
    

    public void HidePath()
    {
        _gridDebuggerParent.SetActive(false);
    }

    private void GetFinalPath(Node startingNode, Node endNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = endNode;

        while (CurrentNode != startingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        FinalPath.Reverse();

        _finalPath = FinalPath;

    }

    private int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int valueX = Mathf.Abs(nodeA.PosX - nodeB.PosX);
        int valueY = Mathf.Abs(nodeA.PosY - nodeB.PosY);

        return valueX + valueY;
        

    }
}
