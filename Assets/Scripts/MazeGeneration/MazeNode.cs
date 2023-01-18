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
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    public void SetState(MazeNodeState state)
    {
        switch (state)
        {
            case MazeNodeState.Available:
                floor.material.color = Color.white;
                break;
            case MazeNodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case MazeNodeState.Completed:
                floor.material.color = Color.blue;
                break;
        }
    }
}
