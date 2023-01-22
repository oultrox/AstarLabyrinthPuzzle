using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamaga.Scripts.AstarPathfinding
{
    public class GridPathFinder : MonoBehaviour, IPathGenerator
    {
        [SerializeField] private GameObject _pathDrawerPrefab;
        private PathDrawer _pathDrawer;
        private Grid _grid;
        private List<Node> _finalPath;
        private bool _isGeneratingPath = false;
      

        #region Properties
        public Grid Grid { get => _grid; set => _grid = value; }
        public bool IsGeneratingPath { get => _isGeneratingPath; }
        #endregion


        private void Awake()
        {
            _grid = GetComponent<Grid>();
            _pathDrawer = Instantiate(_pathDrawerPrefab).GetComponent<PathDrawer>();
        }

        private void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
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
            _pathDrawer.DrawPath(_grid.NodeArray, _finalPath);
        }

        public IEnumerator InitializeGrid()
        {
            _pathDrawer.HidePath();
            yield return null;
            Grid.CreateGrid();
        }

        public IEnumerator FindSolution(Vector3 initialPos, Vector3 targetPos)
        {
            _isGeneratingPath = true;
            _pathDrawer.HidePath();
            _pathDrawer.ClearPath();

            FindPath(initialPos, targetPos);
            _pathDrawer.ShowPath();
            _isGeneratingPath = false;
            yield return null;
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
}


