using MazeGeneration;
using UnityEngine;

namespace AstarPathfinding
{
    public interface IEntitySpawner
    {
        Vector3 PlayerPosition { get; }
        Vector3 TreasurePosition { get; }

        /// <summary>
        /// Either Spawn or move the player.
        /// </summary>
        /// <param name="mazeGenerator"></param>
        void SpawnEntities(IMazeGenerator mazeGenerator);
    }
}