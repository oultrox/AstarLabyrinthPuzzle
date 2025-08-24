

using UnityEngine;

namespace MazeGeneration
{
    public interface IMazeGenerator
    {
        Vector3 GetStartPosition();
        Vector3 GetEndGoalPosition();
    }
}
