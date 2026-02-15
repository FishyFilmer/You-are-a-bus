using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls input;
    Vector2 movement;
    Rigidbody rb;
    [SerializeField] float speed = 3;
    [SerializeField] float rotSpeed = 0.1f;

    private void Awake()
    {
        input = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Gameplay.Move.performed += OnMovement;
        input.Gameplay.Move.canceled += OnMovement;
    }

    private void OnDisable()
    {
        input.Disable(); 
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(movement.x, 0, movement.y) * speed);

        if (movement != new Vector2(0, 0))
        {
            float angle = Mathf.Atan2(movement.y, -movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle - 90, 0), rotSpeed);
        }
    }

}
