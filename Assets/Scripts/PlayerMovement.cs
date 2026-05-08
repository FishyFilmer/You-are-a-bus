using System;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls input; //Allows mapping of player inputs to functions
    Vector2 movement; //Stores the players movement input
    Rigidbody rb; //Bus' rigidbody for physics

    long movementStartTime; //Used to store when the player starts movement input

    [Header("Movement")]
    [Tooltip("Minimum speed for the bus")]
    [SerializeField] float minSpeed = 70f; //Minimum speed for the bus
    [Tooltip("Maximum speed for the bus")]
    [SerializeField] float maxSpeed = 140f; //Maximum speed for the bus
    [Tooltip("Time in milliseconds to go from min to max speed")]
    [SerializeField] float minToMax = 2000f; //Time in milliseconds to go from min to max speed
    [Tooltip("How fast the bus rotates")]
    [SerializeField] float rotSpeed = 0.1f; //How fast the bus rotates

    bool isBoosting = false;
    float currentAcceleration = 0f; //Used to pass speed to other scripts
    int boostTime; //Amount of boost time the player has in milliseconds

    [Header("Boost")]
    [Tooltip("Boost speed multiplier")]
    [SerializeField] float maxBoostMultiplier = 100f; //Boost speed multiplier
    [Tooltip("Max boost time in milliseconds")]
    [SerializeField] int maxBoostTime = 10000; //Max boost time in milliseconds
    [Tooltip("Time for boost to refill in milliseconds")]
    [SerializeField] int boostRefillTime = 5000; //Time for boost to refill in milliseconds
    [Tooltip("Percentage of max boost the player needs to activate boost as a decimal")]
    [SerializeField] float activateBoostAmount = 0.05f; //Percentage of max boost the player needs to activate boost as a decimal

    [Header("Camera")]
    [Tooltip("Players main camera")]
    [SerializeField] Camera playerCamera;
    [Tooltip("Camera positions")]
    [SerializeField] GameObject[] cameraPositions;
    [Tooltip("How fast the camera rotates")]
    [SerializeField] float cameraRotateSpeed = 3.0f;
    [Tooltip("How fast the camera moves")]
    [SerializeField] float cameraMoveSpeed = 3.0f;
    int cameraPosPointer = 0;
    bool changeCamera;
    bool isBigRoomView = true;
    GameObject[] bigRoomCamPos;


    private void Awake()
    {
        //Creates or obtains needed components
        input = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        //
        boostTime = maxBoostTime;
        // print(Time.fixedDeltaTime);

        bigRoomCamPos = cameraPositions;
    }

    private void OnEnable()
    {
        input.Enable(); //Enables inputs

        //Maps player movement input to the functions
        input.Gameplay.Move.started += OnStartMovement;
        input.Gameplay.Move.performed += OnMovement; 
        input.Gameplay.Move.canceled += OnMovement; //Needed so that movement gets set to (0,0) and stops the bus moving
        input.Gameplay.Boost.started += OnStartBoost;
        input.Gameplay.Boost.canceled += OnStopBoost;
        input.Gameplay.CameraLeft.started += OnCameraLeft;
        input.Gameplay.CameraRight.started += OnCameraRight;
    }

    private void OnDisable()
    {
        input.Disable(); //Disables inputs
    }

    private void OnCameraLeft(InputAction.CallbackContext context)
    {
        //Minus
        if (changeCamera == false)
        {
            if (cameraPosPointer - 1 < 0)
            {
                cameraPosPointer = cameraPositions.Length - 1;
            }
            else
            {
                cameraPosPointer -= 1;
            }

            changeCamera = true;
        }
        
        //playerCamera.transform.rotation = cameraPositions[cameraPosPointer].transform.rotation;
        //playerCamera.transform.position = cameraPositions[cameraPosPointer].transform.position;
    }

    private void OnCameraRight(InputAction.CallbackContext context)
    {
        //Plus
        if (changeCamera == false)
        {
            if (cameraPosPointer + 1 > cameraPositions.Length - 1)
            {
                cameraPosPointer = 0;
            }
            else
            {
                cameraPosPointer += 1;
            }

            changeCamera = true;
        }
        


        //playerCamera.transform.rotation = cameraPositions[cameraPosPointer].transform.rotation;
        //playerCamera.transform.position = cameraPositions[cameraPosPointer].transform.position;
    }

    private void OnStartMovement(InputAction.CallbackContext context)
    {
        movementStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //Grabs current unix time in milliseconds
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>(); //Stores the players movement inputs
    }

    private void OnStartBoost(InputAction.CallbackContext context)
    {
        //Activates boost if the player has boost available
        if (boostTime > maxBoostTime * activateBoostAmount) 
        {
            isBoosting = true;
            // print("boost");
        }
    }

    private void OnStopBoost(InputAction.CallbackContext context)
    {
        //Stops boosting if boosting
        if (isBoosting)
        {
            isBoosting = false;
            // print("stop boost");
        }
    }

    private void FixedUpdate()
    {
        //Rotates camera
        if (changeCamera)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraPositions[cameraPosPointer].transform.position, cameraMoveSpeed * Time.fixedDeltaTime);
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, cameraPositions[cameraPosPointer].transform.rotation, cameraMoveSpeed * Time.fixedDeltaTime);

            //playerCamera.transform.position = Vector3.MoveTowards(playerCamera.transform.position, cameraPositions[cameraPosPointer].transform.position, cameraMoveSpeed);
            //playerCamera.transform.rotation = Quaternion.RotateTowards(playerCamera.transform.rotation, cameraPositions[cameraPosPointer].transform.rotation, cameraRotateSpeed);


            if (Vector3.Distance(playerCamera.transform.position, cameraPositions[cameraPosPointer].transform.position) < 0.25f)
            {
                playerCamera.transform.position = cameraPositions[cameraPosPointer].transform.position;
                playerCamera.transform.rotation = cameraPositions[cameraPosPointer].transform.rotation;
                changeCamera = false;
            }
        }
        
        
        //Calculates amount of boost time
        if (!isBoosting && boostTime < maxBoostTime)
        {
            boostTime += (int)(Time.fixedDeltaTime * 1000 * (maxBoostTime / boostRefillTime));
            // print(boostTime);
        }
        else if (isBoosting && boostTime > 0)
        {
            boostTime -= (int)(Time.fixedDeltaTime * 1000);
            if (boostTime <= 0)
            {
                isBoosting = false;
                // print("ran out of boost");
            }
            // print(boostTime);
        }

        //Gets the forward and right vectors of the camera
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        //Creates the movement direction
        Vector3 direction = (forward * movement.y) + (right * movement.x);
        
        //Calculates time difference since movement input started
        long difference = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - movementStartTime; 

        //Calculates acceleration based of difference
        float acceleration = 1;
        if (difference < minToMax)
        {
            acceleration = difference / minToMax;
        }

        //Calculates speed 
        float speed = 0;
        //if (isBoosting)
        //{
        //    speed = (minSpeed + (maxSpeed - minSpeed) * acceleration) * maxBoostMultiplier;
        //}
        //else
        //{
        //    speed = minSpeed + (maxSpeed - minSpeed) * acceleration;
        //}

        ////Applies the movement force
        //rb.AddForce(direction * speed);



        speed = minSpeed + (maxSpeed - minSpeed) * acceleration;

        if (isBoosting)
        {
            rb.AddForce(direction * speed + transform.forward * maxBoostMultiplier);
        }
        else
        {
            rb.AddForce(direction * speed);
        }

        currentAcceleration = (direction * acceleration).magnitude;

        //Rotates the bus in the direction of movement
        if (movement != new Vector2(0, 0)) //Stops the bus from defaulting to a rotation when player input stops
        {
            float angle = Mathf.Atan2(direction.z, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle - 90, 0), rotSpeed);
        }
    }

    public bool GetIsBoosting()
    {
        return isBoosting;
    }

    public float GetCurrentAcceleration()
    {
        return currentAcceleration;
    }

    public void UpdateCamPositions(GameObject[] newCamPos)
    {
        if (isBigRoomView == true)
        {
            // Big room to small
            isBigRoomView = false;
            cameraPositions = newCamPos;
        }
        else
        {
            // Small room to big
            isBigRoomView = true;
            cameraPositions = bigRoomCamPos;
        }

        cameraPosPointer = 0;

        for (int i =1;  i < cameraPositions.Length; i++)
        {
            if(playerCamera.transform.eulerAngles.y == 0 || playerCamera.transform.eulerAngles.y == 180) //y axis
            {
                //print(Math.Abs(playerCamera.transform.position.z - cameraPositions[i].transform.position.z));
                //print(Math.Abs(playerCamera.transform.position.z - cameraPositions[cameraPosPointer].transform.position.z));
                print(cameraPositions[i].transform.position.z);
                print(cameraPositions[cameraPosPointer].transform.position.z);
                if (Math.Abs(playerCamera.transform.position.z - cameraPositions[i].transform.position.z) <
                    Math.Abs(playerCamera.transform.position.z - cameraPositions[cameraPosPointer].transform.position.z))
                {
                    cameraPosPointer = i;
                }
            }
            else if (playerCamera.transform.eulerAngles.y == 90 || playerCamera.transform.eulerAngles.y == 270) //x axis
            {
                //print(Math.Abs(playerCamera.transform.position.x - cameraPositions[i].transform.position.x));
                //print(Math.Abs(playerCamera.transform.position.x - cameraPositions[cameraPosPointer].transform.position.x));
                print(cameraPositions[i].transform.position.x);
                print(cameraPositions[cameraPosPointer].transform.position.x);
                if (Math.Abs(playerCamera.transform.position.x - cameraPositions[i].transform.position.x) <
                    Math.Abs(playerCamera.transform.position.x - cameraPositions[cameraPosPointer].transform.position.x))
                {
                    cameraPosPointer = i;
                }
            }
        }
        changeCamera = true;
    }

}
