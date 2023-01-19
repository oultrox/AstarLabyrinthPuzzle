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
    [SerializeField] private MeshRenderer _floor;

    public void RemoveWall(int wallToRemove)
    {
        _walls[wallToRemove].gameObject.SetActive(false);
    }

    public void SetState(MazeNodeState state)
    {
        switch (state)
        {
            case MazeNodeState.Available:
                _floor.material.color = Color.white;
                break;
            case MazeNodeState.Current:
                _floor.material.color = Color.yellow;
                break;
            case MazeNodeState.Completed:
                _floor.material.color = Color.blue;
                break;
        }
    }
}
