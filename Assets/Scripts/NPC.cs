using System.Net;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{    
    private NavMeshAgent agent;
    private SphereCollider fearRadius;
    private BoxCollider bc;
    private bool closeToPlayer = false;
    private GameObject target;

    public CapsuleCollider capsuleCollider;
    public Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
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
            animator.SetBool("IsRunning", true);
        }
        if (trigger == bc) //Deletes object on collision with player
        {
            animator.SetBool("Caught", true);  
            capsuleCollider.enabled = false;
            closeToPlayer = false;
            agent.enabled = false;
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger == fearRadius) //Unchecks close proximity to player
        {
            closeToPlayer = false;
            animator.SetBool("IsRunning", false);
        }
    }
}
