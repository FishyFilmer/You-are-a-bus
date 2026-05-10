using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private BoxCollider bc;
    private GameObject target;
    private bool shift = false;
    public Camera camera;

    private void Awake()
    {
        target = GameObject.Find("Shift");
        bc = target.GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        if (shift)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(119,15,59), Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger == bc)
        {
            shift = true;
            Destroy(target);
        }
    }
}
