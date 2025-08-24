# Maze Labyrinth Game with A* Pathfinding with Gyroscope!

<img width="731" height="453" alt="image" src="https://github.com/user-attachments/assets/d7448f48-20af-4e2e-9d40-8f8bbe4e64a3" />

## Overview

This project is a **maze labyrinth game** implemented in Unity. Players can generate a random maze and visualize the solution path using an **A * pathfinding algorithm**. The game showcases clean architecture with:

- **Event-driven design** via a custom Event Bus framework (`SimpleBus`).
- **Manual dependency injection** from a single entry point.
- Configurable path visualization with **step-trail rendering**.

---

## Features

- **Random Maze Generation**: Procedurally generates a new maze layout every time the player clicks the "Generate Map" button.
- *A Pathfinding*: Calculates the shortest path from the player to the treasure, using a configurable node grid.
- **Path Visualization**: Draws the path using a step-based trail for easy readability, independent of the maze node precision.
- **Event Bus System**: Clean decoupled architecture using a custom event bus framework (`SimpleBus`) to manage game events.
- **Manual Dependency Injection**: All components are injected from a single entry point (`MazeMatchStart`), keeping the system modular and testable.

---

## How It Works

### Single Entry Point (`MazeMatchStart`)

- Sets up all core systems: `GridPathBuilder`, `PathDrawer`, `EntitySpawner`, `GridPathFinder`, and `MazeGenerator`.
- Registers UI buttons to raise **events** (`MapGeneratedEvent`, `ShowMazePathEvent`).
- Uses the **Event Bus** to notify subscribers when to generate a new maze or show the solution path.

```csharp
 /// <summary>
 /// Single Entry Point layer of the maze test, Manual injector.
 /// </summary>
 public class MazeMatchStart : MonoBehaviour
 {
     [Header("References")]
     [SerializeField] private PathDrawerConfig pathDrawerConfig;
     [SerializeField] private GameObject playerPrefab;
     [SerializeField] private GameObject treasurePrefab;

     [Header("Maze Data")]
     [SerializeField] private MazeNode _mazeNodePrefab;
     [SerializeField] private LayerMask _wallMask;

     [Header("Grid Data")]
     [SerializeField] private Vector2Int _gridWorldSize;

     [Header("GUI")]
     [SerializeField] private Button generateButton;
     [SerializeField] private Button showPathButton;

     private const float NODE_RADIUS = 0.1f;

     private IPathFinder _pathFinder;
     private IEntitySpawner _entitySpawner;
     private IMazeGenerator _mazeGenerator;
     private PathDrawer _pathDrawer;

     private readonly MapGeneratedEvent _mapGenerated = new();
     private readonly ShowMazePathEvent _showMazePath = new();

     void Start()
     {
         SetBehavioralComponents();
         InjectListeners();
     }

     void SetBehavioralComponents()
     {
         var grid = new GridPathBuilder(_wallMask, _gridWorldSize, NODE_RADIUS, transform);
         _pathDrawer = new PathDrawer(pathDrawerConfig);
         _entitySpawner = new EntitySpawner(playerPrefab, treasurePrefab);
         _pathFinder = new GridPathFinder(grid, _pathDrawer, _entitySpawner);
         _mazeGenerator = new MazeGenerator(_pathFinder, _entitySpawner, _gridWorldSize, _mazeNodePrefab);
     }

     void InjectListeners()
     {
         generateButton.onClick.AddListener(GenerateMap);
         showPathButton.onClick.AddListener(ShowPath);
     }

     void GenerateMap()
     {
         EventBus<MapGeneratedEvent>.Raise(_mapGenerated);
     }

     void ShowPath()
     {
         EventBus<ShowMazePathEvent>.Raise(_showMazePath);
     }
 }
```

---

### Path Drawing Logic

- The `PathDrawer` takes the A* path and draws it in the scene using cubes.
- Supports configurable **visual scale** and **node skipping** to render a clear step-trail.
- Uses `ObjectPool<GameObject>` for performance, avoiding frequent instantiation/destruction.

---

### Event System

- The game uses a **generic event listener** (`EventBinder<T>` / `EventListener<T>`) to handle events.
- Components subscribe to events without tight coupling, allowing modular updates and easy testing.

```csharp
_mapGeneratedBinder = new EventListener<MapGeneratedEvent>(Generate);
EventBus<MapGeneratedEvent>.Register(_mapGeneratedBinder);

```

---

## How to Play

1. Click **Generate Map** to create a new maze.
2. Click **Show Path Solution** to visualize the A* path from the player to the treasure.
3. Observe the path trail as a sequence of steps.

---

## Dependencies

- Unity 2021+
- No external packages; all pathfinding and event system implemented in project.

---

## Screenshots

![Maze](https://github.com/user-attachments/assets/d391e65d-07da-4f27-bc54-262731cb8128)

---

## Future Improvements

- Possibly convert it into a full game with procedural levels
- Add better backgrounds and gimmmicks if I get to do it a mobile title.
