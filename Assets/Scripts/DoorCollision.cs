using UnityEngine;

public class DoorCollision : MonoBehaviour
{
    [Tooltip("Camera positions")]
    [SerializeField] GameObject[] cameraPositions;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerMovement>().UpdateCamPositions(cameraPositions);
        }
    }
}
