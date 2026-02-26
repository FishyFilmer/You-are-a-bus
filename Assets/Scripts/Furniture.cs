using UnityEngine;

public class Furniture : MonoBehaviour
{
    void OnCollisionEnter(Collision col) //Deletes object on collision with player
    {
        if (col.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
