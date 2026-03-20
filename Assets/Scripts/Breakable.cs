<<<<<<< HEAD
using System.Collections;
=======
>>>>>>> b90a6225fbc5936cea2dc2666a21f04136812e10
using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactPrefab;
    [SerializeField] GameObject brokenPrefab;

<<<<<<< HEAD
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
=======
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
>>>>>>> b90a6225fbc5936cea2dc2666a21f04136812e10
        {
            Break();
        }
    }

    private void Break()
    {
<<<<<<< HEAD
        brokenPrefab.SetActive(true);
        intactPrefab.SetActive(false);

        bc.enabled = false;
        // for (int i = 0; i < mc.Length - 1; i++)
        // {
        //     mc[i].
        // }
        
        Destroy(gameObject, 5);
=======
        intactPrefab.SetActive(false);
        brokenPrefab.SetActive(true);

        bc.enabled = false;
>>>>>>> b90a6225fbc5936cea2dc2666a21f04136812e10
    }
}