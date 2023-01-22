using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamaga.Scripts.MazeGeneration
{
    public class MazeNode : MonoBehaviour
    {
        [SerializeField] private GameObject[] _walls;

        public void RemoveWall(int wallToRemove)
        {
            _walls[wallToRemove].SetActive(false);
        }

    }
}


