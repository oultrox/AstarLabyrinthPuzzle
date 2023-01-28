using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamaga.Scripts.AstarPathfinding
{
    public class GridPathFinder : MonoBehaviour, IPathGenerator
    {
        [SerializeField] private GameObject _pathDrawerPrefab;
        private AStar _aStarSolution = new AStar();
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
            _finalPath = _aStarSolution.FindPathSolution(initialPos, targetPos, _grid);
            _pathDrawer.DrawPath(_grid.NodeArray, _finalPath);
            _pathDrawer.ShowPath();
            _isGeneratingPath = false;
            yield return null;
        }
    }
}


