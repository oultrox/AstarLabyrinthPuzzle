

using AstarPathfinding;
using UnityEngine;

namespace MazeGeneration
{
    public interface IMazeGenerator
    {
        void Initialize(IPathGenerator pathGenerator, IEntitySpawner entitySpawner);
        Vector3 GetStartPosition();
        Vector3 GetEndGoalPosition();
    }
}
