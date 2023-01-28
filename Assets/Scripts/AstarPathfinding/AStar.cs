using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gamaga.Scripts.AstarPathfinding
{
    public class AStar
    {

        public List<Node> FindPathSolution(Vector3 startPos, Vector3 targetPos, Grid grid)
        {
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();
            List<Node> finalList = new List<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node CurrentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < CurrentNode.FCost || openList[i].FCost == CurrentNode.FCost && openList[i].DistanceCost < CurrentNode.DistanceCost)
                    {
                        CurrentNode = openList[i];
                    }
                }
                openList.Remove(CurrentNode);
                closedList.Add(CurrentNode);

                if (CurrentNode == targetNode)
                {
                    finalList = GetFinalPath(startNode, targetNode);
                }

                //Loop through each neighbor of the current node
                foreach (Node NeighborNode in grid.GetNeighborNode(CurrentNode))
                {
                    if (!NeighborNode.IsWall || closedList.Contains(NeighborNode))
                    {
                        continue;
                    }
                    //Get the F cost of that neighbor
                    int MoveCost = CurrentNode.PathCost + GetManhattenDistance(CurrentNode, NeighborNode);

                    //If the f cost is greater than the g cost or it is not in the open list
                    if (MoveCost < NeighborNode.PathCost || !openList.Contains(NeighborNode))
                    {
                        //Set the g cost to the f cost
                        NeighborNode.PathCost = MoveCost;
                        NeighborNode.DistanceCost = GetManhattenDistance(NeighborNode, targetNode);

                        //Set the parent of the node for retracing steps
                        NeighborNode.ParentNode = CurrentNode;

                        if (!openList.Contains(NeighborNode))
                        {
                            openList.Add(NeighborNode);
                        }
                    }
                }
            }
            return finalList;
        }

        private List<Node> GetFinalPath(Node startingNode, Node endNode)
        {
            List<Node> finalPath = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startingNode)
            {
                finalPath.Add(currentNode);
                currentNode = currentNode.ParentNode;
            }

            finalPath.Reverse();

            return finalPath;

        }

        private int GetManhattenDistance(Node nodeA, Node nodeB)
        {
            int valueX = Mathf.Abs(nodeA.PosX - nodeB.PosX);
            int valueY = Mathf.Abs(nodeA.PosY - nodeB.PosY);
            return valueX + valueY;
        }

    }
}

