using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeNodeState
{
    Available,
    Current,
    Completed
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] private GameObject[] _walls;

    public void RemoveWall(int wallToRemove)
    {
        _walls[wallToRemove].gameObject.SetActive(false);
    }

}
