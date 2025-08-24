using Gamaga.Scripts.AstarPathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _pathRender;
    [SerializeField] private bool _isUsingPool;
    private GameObject _gridDebuggerParent;
    private const string PATH_DEBUGGER = "PathDebugger";
    private Vector3 _pathPosition;
    private ObjectPool<GameObject> _pathPool;


    private void Awake()
    {
        _gridDebuggerParent = new GameObject();
        _gridDebuggerParent.transform.SetParent(this.transform);
        _gridDebuggerParent.name = PATH_DEBUGGER;

        _pathPool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(_pathRender);
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
                Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
        }
    }

    internal void HidePath()
    {
        _gridDebuggerParent.SetActive(false);
    }

    internal void DrawPath(Node[,] nodeArray, List<Node> finalTarget)
    {
        if (nodeArray != null)
        {
            if(_pathPool.CountActive > 0)
                ClearPath();
            foreach (Node n in nodeArray)
            {
                if (finalTarget == null)
                {
                    continue;
                }

                if (finalTarget.Contains(n))
                {
                    SpawnPathInNode(n);

                }
            }
        }
    }

    private void SpawnPathInNode(Node n)
    {
        _pathPosition = n.Position;
        _pathPosition.y = 0.02f;
        if (_isUsingPool)
        {
            var prefabPath = _pathPool.Get();
            prefabPath.transform.position = _pathPosition;
            prefabPath.transform.parent = _gridDebuggerParent.transform;
        }
        else
        {
            Instantiate(_pathRender, _pathPosition, Quaternion.identity, _gridDebuggerParent.transform);
        }
    }
}
