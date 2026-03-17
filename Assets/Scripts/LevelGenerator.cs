using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Mathematics;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    [Tooltip("Drag different room prefabs here")]
    public List<GameObject> roomPrefabs = new List<GameObject>();

    [Header("Generation Settings")]
    [Tooltip("How many rooms in level")]
    public int numberOfRooms = 10;

    [Header("NavMeshSurface")]
    public NavMeshSurface surface;

    // [Tooltip("Height of each room")]
    // public float roomHeight = 10f;

    // [Tooltip("X position where the bottom of the first room spawns")]
    private float startX = 0f;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private List<GameObject> spawnedRooms = new List<GameObject>();

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // Clears any previously spawned rooms
        foreach (var room in spawnedRooms)
        {
            if (room != null) Destroy(room);
        }
        spawnedRooms.Clear();

        if (roomPrefabs == null || roomPrefabs.Count == 0)
        {
            return;
        }

        // Shuffles a copy of the prefab list
        var shuffledPrefabs = roomPrefabs.ToList();
        shuffledPrefabs.Shuffle();

        float startX = 0f;
        int j = 0;

        for (int i = 0; i < numberOfRooms; i++)
        {
            // Cycle through the shuffled list if we run out of unique prefabs
            int prefabIndex = i % shuffledPrefabs.Count;
            GameObject prefabToUse = shuffledPrefabs[prefabIndex];

            // If statement ensures generated rooms dont extend past 5 in a row
            if (i > 4)
            {
                spawnPosition = new Vector3(startX+(25f*j), -1.3f, 60f);
                spawnRotation = Quaternion.Euler(0, 180, 0);
                j++;
            }
            else
            {
                spawnPosition = new Vector3(startX+(25f*i), -1.3f, 0f);
                spawnRotation = Quaternion.Euler(0, 0, 0);
            }

            GameObject newRoom = Instantiate(prefabToUse, spawnPosition, spawnRotation, transform);
            spawnedRooms.Add(newRoom);

            // currentX += roomHeight;
        }

        surface.BuildNavMesh();
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}