using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls input; //Allows mapping of player inputs to functions
    Vector2 movement; //Stores the players movement input
    Rigidbody rb; //Bus' rigidbody for physics
    long startTime; //Used to store when the player starts movement input

    [SerializeField] Camera curCamera; //The current camera

    [SerializeField] float minSpeed = 30f; //Minimum speed for the bus
    [SerializeField] float maxSpeed = 70f; //Maximum speed for the bus
    [SerializeField] float noughtToMax = 4400f; //Time in milliseconds to go from min to max speed
    [SerializeField] float rotSpeed = 0.1f; //How fast the bus rotates

    private void Awake()
    {
        //Creates or obtains needed components
        input = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        input.Enable(); //Enables inputs

        //Maps player movement input to the functions
        input.Gameplay.Move.started += OnStartMovement;
        input.Gameplay.Move.performed += OnMovement; 
        input.Gameplay.Move.canceled += OnMovement; //Needed so that movement gets set to (0,0) and stops the bus moving
    }

    private void OnDisable()
    {
        input.Disable(); //Disables inputs
    }

    private void OnStartMovement(InputAction.CallbackContext context)
    {
        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //Grabs the time when the player starts moving
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>(); //Stores the players movement inputs
    }

    private void FixedUpdate()
    {
        //Gets the forward and right vectors of the camera
        Vector3 forward = curCamera.transform.forward;
        Vector3 right = curCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        //Creates the movement direction
        Vector3 direction = (forward * movement.y) + (right * movement.x);
        
        //Calculates time difference since movement input started
        long difference = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime; 

        //Calculates speed multiplier based off difference
        float speedMultiplier = 1;
        if (difference < noughtToMax)
        {
            speedMultiplier = difference / noughtToMax;
        }

        //Calculates speed based off how long the player has been moving (i.e. acceleration)
        float speed = minSpeed + (maxSpeed - minSpeed) * speedMultiplier;

        //Applies the movement force
        rb.AddForce(direction * speed);

        //Rotates the bus in the direction of movement
        if (movement != new Vector2(0, 0)) //Stops the bus from defaulting to a rotation when player input stops
        {
            float angle = Mathf.Atan2(direction.z, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle - 90, 0), rotSpeed);
        }
    }

}
