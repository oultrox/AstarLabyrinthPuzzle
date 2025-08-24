using System.Collections.Generic;
using System.Threading.Tasks;
using Gamaga.Scripts.AstarPathfinding;

namespace AstarPathfinding
{
    public interface IPathFinder
    {
        bool IsGeneratingPath { get; }
        Task InitializeGridAsync();
        Task<List<Node>> FindSolutionAsync();
    }
}