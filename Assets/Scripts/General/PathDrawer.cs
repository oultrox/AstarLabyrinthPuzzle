using Gamaga.Scripts.AstarPathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private GameObject pathRender;
    private GameObject _gridDebuggerParent;
    private const string PATH_DEBUGGER = "PathDebugger";
    private Vector3 _pathPosition;

    private void Awake()
    {
        _gridDebuggerParent = new GameObject();
        _gridDebuggerParent.transform.SetParent(this.transform);
        _gridDebuggerParent.name = PATH_DEBUGGER;
        _gridDebuggerParent.SetActive(false);
    }

    internal void ShowPath()
    {
        _gridDebuggerParent.SetActive(true);
    }

    internal void ClearPath()
    {
        for (var i = _gridDebuggerParent.transform.childCount - 1; i >= 0; i--)
        {
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
            ClearPath();
            foreach (Node n in nodeArray)
            {
                if (finalTarget == null)
                {
                    continue;
                }

                if (finalTarget.Contains(n))
                {
                    _pathPosition = n.Position;
                    _pathPosition.y = 0.02f;
                    Instantiate(pathRender, _pathPosition, Quaternion.identity, _gridDebuggerParent.transform);
                }
            }
        }
    }
}
