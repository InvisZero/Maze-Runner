using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [Header("References")]
    public GameObject wallPrefab;
    public GameObject exitPortalPrefab;
    public GameObject checkpointMaterialXPrefab;
    public GameObject checkpointMaterialYPrefab;
    public GameObject checkpointMaterialZPrefab;
    public GameObject robotPrefab;
    public Transform mazeParent;

    [Header("Maze Settings")]
    public static int width = 21;
    public static int height = 21;

    [Header("Visual")]
    public static float tileSize = 2f;

    public static int[,] maze;

    private List<Vector2Int> walkableCells = new List<Vector2Int>();
    private Vector2Int playerCell = new Vector2Int(1, 1);
    private Vector2Int exitCell;

    void Start()
    {
        GenerateMaze();
        DrawMaze();

        SpawnExit();
        SpawnCheckpointMaterials(checkpointMaterialXPrefab);
        SpawnCheckpointMaterials(checkpointMaterialYPrefab);
        SpawnCheckpointMaterials(checkpointMaterialZPrefab);

        SpawnRobots();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int start = playerCell;

        maze[start.x, start.y] = 0;
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();

            List<Vector2Int> neighbours = new List<Vector2Int>();

            Vector2Int[] directions =
            {
                new Vector2Int(0,2),
                new Vector2Int(0,-2),
                new Vector2Int(2,0),
                new Vector2Int(-2,0)
            };

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                if (next.x > 0 &&
                    next.x < width - 1 &&
                    next.y > 0 &&
                    next.y < height - 1 &&
                    maze[next.x, next.y] == 1)
                {
                    neighbours.Add(next);
                }
            }

            if (neighbours.Count > 0)
            {
                Vector2Int chosen =
                    neighbours[Random.Range(0, neighbours.Count)];

                Vector2Int wall =
                    current + ((chosen - current) / 2);

                maze[wall.x, wall.y] = 0;
                maze[chosen.x, chosen.y] = 0;

                stack.Push(chosen);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    void DrawMaze()
    {
        foreach (Transform child in mazeParent)
        {
            Destroy(child.gameObject);
        }

        walkableCells.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Instantiate(
                        wallPrefab,
                        CellToWorld(new Vector2Int(x, y)),
                        Quaternion.identity,
                        mazeParent
                    );
                }
                else
                {
                    walkableCells.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * tileSize,
            cell.y * tileSize,
            0
        );
    }

    void SpawnExit()
    {
        float farthestDistance = -1f;

        foreach (Vector2Int cell in walkableCells)
        {
            float d = Vector2.Distance(playerCell, cell);

            if (d > farthestDistance)
            {
                farthestDistance = d;
                exitCell = cell;
            }
        }

        Instantiate(
            exitPortalPrefab,
            CellToWorld(exitCell),
            Quaternion.identity
        );
    }

    void SpawnCheckpointMaterials(GameObject prefab)
{
    List<Vector2Int> available = new List<Vector2Int>(walkableCells);

    available.Remove(playerCell);
    available.Remove(exitCell);

    int spawned = 0;

    while (spawned < 2 && available.Count > 0)
    {
        int index = Random.Range(0, available.Count);

        Vector2Int cell = available[index];

        float playerDistance = Vector2.Distance(playerCell, cell);
        float exitDistance = Vector2.Distance(exitCell, cell);

        if (playerDistance > 5f &&
            exitDistance > 5f)
        {
            Instantiate(
                prefab,
                CellToWorld(cell),
                Quaternion.identity
            );

            spawned++;
        }

        available.RemoveAt(index);
    }
}

void SpawnRobots()
{
    List<Vector2Int> available = new List<Vector2Int>(walkableCells);

    available.Remove(playerCell);
    available.Remove(exitCell);

    int robotsToSpawn = SeedManager.worldLevel + 1;
    int spawned = 0;

    while (spawned < robotsToSpawn && available.Count > 0)
    {
        int index = Random.Range(0, available.Count);

        Vector2Int cell = available[index];

        float playerDistance = Vector2.Distance(playerCell, cell);
        float exitDistance = Vector2.Distance(exitCell, cell);

        if (playerDistance > 8f &&
            exitDistance > 8f)
        {
            Instantiate(
                robotPrefab,
                CellToWorld(cell),
                Quaternion.identity
            );

            spawned++;
        }

        available.RemoveAt(index);
    }
}
}