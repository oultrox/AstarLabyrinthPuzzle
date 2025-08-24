using MazeGeneration;
using UnityEngine;

namespace AstarPathfinding
{
    public interface IEntitySpawner
    {
        Vector3 PlayerPosition { get; }
        Vector3 TreasurePosition { get; }

        // Spawn or move the player to a position
        void SpawnEntities(IMazeGenerator mazeGenerator);
    }
}