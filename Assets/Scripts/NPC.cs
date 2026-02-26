using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{    
    private NavMeshAgent agent;
    private SphereCollider fearRadius;
    private BoxCollider bc;
    private bool closeToPlayer = false;
    [SerializeField] private GameObject target; //Serialized variable for player object

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        fearRadius = target.GetComponent<SphereCollider>();
        bc = target.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Vector3 directionToPlayer = target.transform.position - transform.position;
        Vector3 oppositeDirection = transform.position - directionToPlayer;

        if (closeToPlayer) //Sets target destination directly opposite to player position
        {
            agent.SetDestination(oppositeDirection);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger == fearRadius) //Checks close proximity to player
        {
            closeToPlayer = true;
        }
        if (trigger == bc) //Deletes object on collision with player
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger == fearRadius) //Unchecks close proximity to player
        {
            closeToPlayer = false;
        }
    }
}
