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
        _gridDebuggerParent.SetActive(false);

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
            Destroy(GameObject);
        }, false, 50, 200);
    }

    internal void ShowPath()
    {
        _gridDebuggerParent.SetActive(true);
    }

    internal void ClearPath()
    {
        for (var i = _gridDebuggerParent.transform.childCount - 1; i >= 0; i--)
        {
            if (!_isUsingPool)
                Destroy(_gridDebuggerParent.transform.GetChild(i).gameObject);
            else
                _pathPool.Release(_gridDebuggerParent.transform.GetChild(i).gameObject);
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
            var pathSpawned = _pathPool.Get();
            pathSpawned.transform.position = _pathPosition;
        }
        else
        {
            Instantiate(_pathRender, _pathPosition, Quaternion.identity, _gridDebuggerParent.transform);
        }
    }
}
