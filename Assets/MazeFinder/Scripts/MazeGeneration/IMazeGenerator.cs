

using AstarPathfinding;
using UnityEngine;

namespace MazeGeneration
{
    public interface IMazeGenerator
    {
        void Initialize(IPathFinder pathFinder, IEntitySpawner entitySpawner);
        Vector3 GetStartPosition();
        Vector3 GetEndGoalPosition();
    }
}
