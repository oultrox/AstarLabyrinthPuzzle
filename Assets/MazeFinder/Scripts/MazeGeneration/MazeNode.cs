using System.Linq;
using UnityEngine;


namespace Gamaga.Scripts.MazeGeneration
{
    public class MazeNode : MonoBehaviour
    {
        private GameObject[] _walls;
        
        public void Init()
        {
            _walls = GetComponentsInChildren<Transform>()
                .Where(t => t != transform)
                .Select(t => t.gameObject)
                .ToArray();    
        }
        
        public void RemoveWall(int wallToRemove)
        {
            _walls[wallToRemove].SetActive(false);
        }

    }
}


