using System.Collections;
using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactPrefab;
    [SerializeField] GameObject[] brokenPrefabs;

    [Header("NPC Spawn Settings")]
    [SerializeField] GameObject npcPrefab;
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.5f; // 50% default

    BoxCollider bc;
    MeshCollider[] mc;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        mc = GetComponentsInChildren<MeshCollider>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            Break();
        }
    }

    private void Break()
    {
        // picks one at random
        int index = Random.Range(0, brokenPrefabs.Length);
        brokenPrefabs[index].SetActive(true);

        intactPrefab.SetActive(false);
        bc.enabled = false;

        // spawns NPC with chance
        if (npcPrefab != null && Random.value <= spawnChance)
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 5); // destroys object
    }
}