using UnityEngine;
using System.Collections.Generic;

public class ProceduralGeneration : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float scale = 0.1f;
    public float threshold = 0.5f;

    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;

    private GameObject player;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool[,] levelGrid;

    private void Start()
    {
        GenerateLevel();
        PlacePlayer();
    }

    void GenerateLevel()
    {
        levelGrid = new bool[width, height];

        // Generate the level
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);

                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                    levelGrid[x, y] = true; // Wall
                }
                else
                {
                    float perlinValue = Mathf.PerlinNoise(x * scale, y * scale);

                    if (perlinValue > threshold)
                    {
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        levelGrid[x, y] = true; // Wall
                    }
                    else
                    {
                        Instantiate(floorPrefab, pos, Quaternion.identity);
                        levelGrid[x, y] = false; // Floor
                    }
                }
            }
        }

        // Create start and end points
        PlaceStartAndEndPoints();
    }

    void PlaceStartAndEndPoints()
    {
        startPoint = GetRandomFloorPosition();
        endPoint = GetRandomFloorPosition();

        while (Vector3.Distance(startPoint, endPoint) < Mathf.Max(width, height) / 2)
        {
            endPoint = GetRandomFloorPosition();
        }

        Instantiate(startPointPrefab, startPoint, Quaternion.identity);
        Instantiate(endPointPrefab, endPoint, Quaternion.identity);
    }

    Vector3 GetRandomFloorPosition()
    {
        int x, y;
        do
        {
            x = Random.Range(1, width - 1);
            y = Random.Range(1, height - 1);
        }
        while (levelGrid[x, y]); // Ensure it's a floor, not a wall

        return new Vector3(x, y, 0);
    }

    void PlacePlayer()
    {
        player = Instantiate(playerPrefab, startPoint, Quaternion.identity);
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, endPoint) < 0.5f)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            GenerateLevel();
            player.transform.position = startPoint;
        }
    }
}
