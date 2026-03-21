using System.Collections;
using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactPrefab;
    [SerializeField] GameObject brokenPrefab;

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
        brokenPrefab.SetActive(true);
        intactPrefab.SetActive(false);

        bc.enabled = false;
        // for (int i = 0; i < mc.Length - 1; i++)
        // {
        //     mc[i].
        // }
        
        Destroy(gameObject, 5);
    }
}