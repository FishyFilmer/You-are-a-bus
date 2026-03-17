using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactPrefab;
    [SerializeField] GameObject brokenPrefab;

    [SerializeField] BoxCollider playerCollider; // Drag the player's box collider

    BoxCollider bc;

    private void Awake()
    {
        intactPrefab.SetActive(true);
        brokenPrefab.SetActive(false);

        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            Break();
        }
    }

    private void Break()
    {
        intactPrefab.SetActive(false);
        brokenPrefab.SetActive(true);

        bc.enabled = false;
    }
}