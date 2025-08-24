using Gamaga.Scripts.AstarPathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PathDrawer
{
    private float _visualScale = 0.15f;
    private GameObject _pathRender;
    private int _skipNodes = 2;
    
    private readonly bool _isUsingPool = true;
    private GameObject _gridDebuggerParent;
    private const string PATH_DEBUGGER = "PathDebugger";
    private Vector3 _pathPosition;
    private ObjectPool<GameObject> _pathPool;
    private Vector3 _transformLocalScale;

    public PathDrawer(PathDrawerConfig config)
    {
        _visualScale = config.VisualScale;
        _pathRender = config.PathRender;
        _skipNodes = config.SkipNodes;
        _gridDebuggerParent = new GameObject();
        _transformLocalScale = new Vector3(_visualScale, _visualScale, _visualScale);
        

        _pathPool = new ObjectPool<GameObject>(() =>
        {
            return Object.Instantiate(_pathRender);
        }, GameObject =>
        {
            GameObject.SetActive(true);
        }, GameObject =>
        {
            GameObject.SetActive(false);
        }, GameObject =>
        {
            GameObject.SetActive(false);
        }, false, 350, 1000);
    }

    internal void ShowPath()
    {
        _gridDebuggerParent.SetActive(true);
    }

    internal void ClearPath()
    {
        for (var i = _pathPool.CountActive - 1; i >= 0; i--)
        {
            if (_isUsingPool)
                _pathPool.Release(_gridDebuggerParent.transform.GetChild(i).gameObject);
            else
                Object.Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
        }
    }

    internal void HidePath()
    {
        _gridDebuggerParent.SetActive(false);
    }

    internal void DrawPath(List<Node> finalTarget)
    {
        if (finalTarget == null || finalTarget.Count == 0)
            return;

        if(_pathPool.CountActive > 0)
            ClearPath();

        for (int i = 0; i < finalTarget.Count; i += _skipNodes)
        {
            SpawnPathInNode(finalTarget[i]);
        }

        // Always draw the last node to make sure the destination is visible
        if (finalTarget.Count > 0 && (finalTarget.Count - 1) % _skipNodes != 0)
        {
            SpawnPathInNode(finalTarget[finalTarget.Count - 1]);
        }
    }   
    
    void SpawnPathInNode(Node n)
    {
        _pathPosition = n.Position;
        _pathPosition.y = 0.02f; // keep it slightly above ground

        GameObject prefabPath;

        if (_isUsingPool)
        {
            prefabPath = _pathPool.Get();
            prefabPath.transform.parent = _gridDebuggerParent.transform;
        }
        else
        {
            prefabPath = Object.Instantiate(_pathRender, _gridDebuggerParent.transform);
        }

        prefabPath.transform.position = _pathPosition;
        
        prefabPath.transform.localScale = _transformLocalScale; 
    }

}