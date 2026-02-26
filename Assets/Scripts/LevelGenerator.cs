using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    [Tooltip("Drag different room prefabs here")]
    public List<GameObject> roomPrefabs = new List<GameObject>();

    [Header("Generation Settings")]
    [Tooltip("How many rooms to stack vertically")]
    public int numberOfRooms = 10;

    [Tooltip("Height of each room")]
    public float roomHeight = 10f;

    [Tooltip("Y position where the bottom of the first room spawns")]
    public float startY = 0f;

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

        float currentY = startY;

        for (int i = 0; i < numberOfRooms; i++)
        {
            // Cycle through the shuffled list if we run out of unique prefabs
            int prefabIndex = i % shuffledPrefabs.Count;
            GameObject prefabToUse = shuffledPrefabs[prefabIndex];

            Vector3 spawnPosition = new Vector3(0f, currentY, 0f);

            GameObject newRoom = Instantiate(prefabToUse, spawnPosition, Quaternion.identity, transform);
            spawnedRooms.Add(newRoom);

            currentY += roomHeight;
        }
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