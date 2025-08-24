using System.Collections.Generic;
using System.Threading.Tasks;
using Gamaga.Scripts.AstarPathfinding;
using MazeFinder.Scripts.Events;
using SimpleBus;
using UnityEngine;
using Grid = Gamaga.Scripts.AstarPathfinding.Grid;

namespace AstarPathfinding
{
    public class GridPathFinder : IPathGenerator
    {
        private AStar _aStar = new();
        private PathDrawer _pathDrawer;
        private Grid _grid;
        private IEntitySpawner _spawner;
        private EventBinding<ShowMazePathEvent> _showMazePathBinding;
        
        public bool IsGeneratingPath { get; private set; }
        
        
        public GridPathFinder(Grid grid, PathDrawer pathDrawer, IEntitySpawner spawner)
        {
            _grid = grid;
            _pathDrawer = pathDrawer;
            _spawner = spawner;
            RegisterEvents();
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

        public async Task<List<Node>> FindSolutionAsync()
        {
            Vector3 start = _spawner.PlayerPosition;
            Vector3 end = _spawner.TreasurePosition;

            var finalPath = _aStar.FindPathSolution(start, end, _grid);
            _pathDrawer.DrawPath(_grid.NodeArray, finalPath);

            await Task.Yield(); // simulate async
            return finalPath;
        }
        
        void RegisterEvents()
        {
            _showMazePathBinding = new EventBinding<ShowMazePathEvent>(() =>
            {
                _ = FindSolutionAsync().ContinueWith(t =>
                {
                    if (t.Exception != null)
                        UnityEngine.Debug.LogException(t.Exception);
                });
            });

            EventBus<ShowMazePathEvent>.Register(_showMazePathBinding);
        }
        
    }
}