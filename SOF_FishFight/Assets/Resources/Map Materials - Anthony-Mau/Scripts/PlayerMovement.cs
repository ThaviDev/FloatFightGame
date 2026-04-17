using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float verticalSpeed = 5f;

    [Header("Floating Behavior")]
    public float sinkForce = -2f;     // constant downward drift
    public float drag = 2f;           // slows movement over time

    private CharacterController controller;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        ApplyFloating();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Horizontal movement
        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;

        // Vertical input (swim up)
        if (Input.GetKey(KeyCode.Space))
        {
            velocity.y = verticalSpeed;
        }
    }

    void ApplyFloating()
    {
        // Constant sinking
        velocity.y += sinkForce * Time.deltaTime;

        // Apply drag (smooth slowdown)
        velocity = Vector3.Lerp(velocity, Vector3.zero, drag * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }
}