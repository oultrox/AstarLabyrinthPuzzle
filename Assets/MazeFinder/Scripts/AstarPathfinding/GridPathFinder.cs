using System.Collections.Generic;
using System.Threading.Tasks;
using Gamaga.Scripts.AstarPathfinding;
using UnityEngine;
using Grid = Gamaga.Scripts.AstarPathfinding.Grid;

namespace AstarPathfinding
{
    public class GridPathFinder : IPathGenerator
    {
        private AStar _aStar = new();
        private PathDrawer _pathDrawer;
        private Grid _grid;
        public bool IsGeneratingPath { get; private set; }
    
        public GridPathFinder(Grid grid, PathDrawer pathDrawer)
        {
            _grid = grid;
            _pathDrawer = pathDrawer;
        }
    
        public async Task InitializeGridAsync()
        {
            IsGeneratingPath = true;
            _pathDrawer.ClearPath();
            
            // Simulate async grid creation without blocking
            await Task.Yield();
            _grid.CreateGrid();

            IsGeneratingPath = false;
        }

        public async Task<List<Node>> FindSolutionAsync(IEntitySpawner spawner)
        {
            Vector3 start = spawner.PlayerPosition;
            Vector3 end = spawner.TreasurePosition;

            var finalPath = _aStar.FindPathSolution(start, end, _grid);
            _pathDrawer.DrawPath(_grid.NodeArray, finalPath);

            await Task.Yield(); // simulate async
            return finalPath;
        }
    }
}