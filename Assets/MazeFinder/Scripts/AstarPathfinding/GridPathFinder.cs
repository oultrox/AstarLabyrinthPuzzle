using System.Collections.Generic;
using System.Threading.Tasks;
using Gamaga.Scripts.AstarPathfinding;
using MazeFinder.Scripts.Events;
using SimpleBus;
using UnityEngine;

namespace AstarPathfinding
{
    public class GridPathFinder : IPathFinder
    {
        private AStar _aStar = new();
        private PathDrawer _pathDrawer;
        private GridPathBuilder _gridBuilder;
        private IEntitySpawner _spawner;
        private EventListener<ShowMazePathEvent> _showMazePathListener;
        
        public bool IsGeneratingPath { get; private set; }
        
        
        public GridPathFinder(GridPathBuilder gridBuilder, PathDrawer pathDrawer, IEntitySpawner spawner)
        {
            _gridBuilder = gridBuilder;
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
            _gridBuilder.CreateGrid();

            IsGeneratingPath = false;
        }

        public async Task<List<Node>> FindSolutionAsync()
        {
            Vector3 start = _spawner.PlayerPosition;
            Vector3 end = _spawner.TreasurePosition;

            var finalPath = _aStar.FindPathSolution(start, end, _gridBuilder);
            _pathDrawer.DrawPath(finalPath);

            await Task.Yield(); // simulate async
            return finalPath;
        }
        
        void RegisterEvents()
        {
            _showMazePathListener = new EventListener<ShowMazePathEvent>(() =>
            {
                _ = FindSolutionAsync().ContinueWith(t =>
                {
                    if (t.Exception != null)
                        Debug.LogException(t.Exception);
                });
            });

            EventBus<ShowMazePathEvent>.Register(_showMazePathListener);
        }
        
    }
}