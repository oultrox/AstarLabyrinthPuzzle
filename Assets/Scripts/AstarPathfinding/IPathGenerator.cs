using System.Collections;
using UnityEngine;

namespace Gamaga.Scripts.AstarPathfinding
{
    public interface IPathGenerator
    {
        bool IsGeneratingPath { get; }
        IEnumerator InitializeGrid();
        IEnumerator FindSolution(Vector3 startPos, Vector3 endPos);
    }
}