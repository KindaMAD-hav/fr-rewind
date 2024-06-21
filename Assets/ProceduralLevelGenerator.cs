using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int width = 100; // Increased level width
    public int height = 100; // Increased level height
    public int roomCount = 20;
    public int minRoomSize = 7;
    public int maxRoomSize = 15;
    public int corridorWidth = 4;
    public int baseEnemyCount = 5; // Base number of enemies
    public float enemyIncreaseRate = 0.2f; // 20% increase per level
    public int currentLevel = 1; // Track the current level

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject playerPrefab; // Add player prefab reference
    public GameObject[] enemyPrefabs; // Array of enemy prefabs

    private int[,] map;
    private GameObject currentPlayerInstance;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // Destroy existing level and player
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        if (currentPlayerInstance != null)
        {
            Destroy(currentPlayerInstance);
        }

        map = new int[width, height];

        // Step 1: Fill the map with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 1; // 1 represents wall
            }
        }

        // Step 2: Create rooms
        List<Rect> rooms = new List<Rect>();
        for (int i = 0; i < roomCount; i++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int roomX = Random.Range(1, width - roomWidth - 1);
            int roomY = Random.Range(1, height - roomHeight - 1);

            Rect newRoom = new Rect(roomX, roomY, roomWidth, roomHeight);

            bool overlaps = false;
            foreach (Rect room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
                for (int x = roomX; x < roomX + roomWidth; x++)
                {
                    for (int y = roomY; y < roomY + roomHeight; y++)
                    {
                        map[x, y] = 0; // 0 represents floor
                    }
                }
            }
        }

        // Step 3: Create corridors
        for (int i = 1; i < rooms.Count; i++)
        {
            int startX = (int)rooms[i - 1].center.x;
            int startY = (int)rooms[i - 1].center.y;
            int endX = (int)rooms[i].center.x;
            int endY = (int)rooms[i].center.y;

            CreateCorridor(startX, startY, endX, endY);
        }

        // Step 4: Place start and end points as far apart as possible
        Vector2 startPos = rooms[0].center;
        Vector2 endPos = startPos;
        float maxDistance = 0f;

        foreach (var roomA in rooms)
        {
            foreach (var roomB in rooms)
            {
                float distance = Vector2.Distance(roomA.center, roomB.center);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    startPos = roomA.center;
                    endPos = roomB.center;
                }
            }
        }

        map[(int)startPos.x, (int)startPos.y] = 2; // 2 represents start point
        map[(int)endPos.x, (int)endPos.y] = 3; // 3 represents end point

        // Step 5: Instantiate the level
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                if (map[x, y] == 0)
                {
                    Instantiate(floorPrefab, position, Quaternion.identity, transform);
                }
                else if (map[x, y] == 1)
                {
                    Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }
                else if (map[x, y] == 2)
                {
                    Instantiate(startPrefab, position, Quaternion.identity, transform);
                }
                else if (map[x, y] == 3)
                {
                    Instantiate(endPrefab, position, Quaternion.identity, transform);
                }
            }
        }

        // Step 6: Instantiate the player at the start point
        Vector3 startPosition = new Vector3((int)startPos.x, (int)startPos.y, 0);
        currentPlayerInstance = Instantiate(playerPrefab, startPosition, Quaternion.identity);

        // Step 7: Create an outer boundary of walls
        CreateOuterBoundary();

        // Step 8: Spawn enemies
        SpawnEnemies(startPosition);
    }

    private void CreateCorridor(int startX, int startY, int endX, int endY)
    {
        while (startX != endX)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                for (int j = 0; j < corridorWidth; j++)
                {
                    int x = Mathf.Clamp(startX + i, 0, width - 1);
                    int y = Mathf.Clamp(startY + j, 0, height - 1);
                    map[x, y] = 0;
                }
            }
            startX += startX < endX ? 1 : -1;
        }

        while (startY != endY)
        {
            for (int i = 0; i < corridorWidth; i++)
            {
                for (int j = 0; j < corridorWidth; j++)
                {
                    int x = Mathf.Clamp(startX + i, 0, width - 1);
                    int y = Mathf.Clamp(startY + j, 0, height - 1);
                    map[x, y] = 0;
                }
            }
            startY += startY < endY ? 1 : -1;
        }
    }

    private void CreateOuterBoundary()
    {
        for (int x = 0; x < width; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(x, height - 1, 0), Quaternion.identity, transform);
        }
        for (int y = 0; y < height; y++)
        {
            Instantiate(wallPrefab, new Vector3(0, y, 0), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(width - 1, y, 0), Quaternion.identity, transform);
        }
    }

    private void SpawnEnemies(Vector3 playerStartPosition)
    {
        int enemyCount = Mathf.FloorToInt(baseEnemyCount * Mathf.Pow(1 + enemyIncreaseRate, currentLevel - 1));
        List<Vector2> availablePositions = new List<Vector2>();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (map[x, y] == 0 && Vector2.Distance(playerStartPosition, new Vector2(x, y)) > 15f)
                {
                    availablePositions.Add(new Vector2(x, y));
                }
            }
        }

        for (int i = 0; i < enemyCount; i++)
        {
            if (availablePositions.Count == 0)
                break;

            Vector2 randomPosition = availablePositions[Random.Range(0, availablePositions.Count)];
            availablePositions.Remove(randomPosition);

            Vector3 enemyPosition = new Vector3(randomPosition.x, randomPosition.y, 0);
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, transform);
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        GenerateLevel();
    }
}
