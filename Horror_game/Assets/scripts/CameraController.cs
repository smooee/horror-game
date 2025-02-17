using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float crawlSpeed = 3f;       // Speed of crawling movement
    public float lookSensitivity = 2f; // Mouse sensitivity
    public float headBobFrequency = 2f; // Frequency of head bobbing
    public float headBobAmount = 0.1f; // Amount of head bobbing

    private Vector3 headBobOffset = Vector3.zero; // Offset for head bobbing
    private float headBobTimer = 0f;

    private Vector3 movementDirection; // Current movement direction
    private bool isInsidePipe = true;  // Whether the player is inside a pipe

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleHeadBob();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W)) // Move forward
        {
            // Move forward relative to the camera's local forward direction
            movementDirection = transform.forward;
            transform.position += movementDirection * crawlSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S)) // Move backward
        {
            // Move backward relative to the camera's local backward direction
            movementDirection = -transform.forward;
            transform.position += movementDirection * crawlSpeed * Time.deltaTime;
        }

        // Toggle pipe exit if moving backward for long enough
        if (Input.GetKey(KeyCode.S) && !isInsidePipe)
        {
            isInsidePipe = false; // Simulate exiting the pipe
        }
    }

    void HandleLook()
    {
        // Mouse look (adjust rotation)
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Adjust pitch (up/down) and clamp it to prevent flipping
        Vector3 rotation = transform.localEulerAngles;
        rotation.x -= mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        // Adjust yaw (left/right)
        rotation.y += mouseX;

        transform.localEulerAngles = rotation;
    }

    void HandleHeadBob()
    {
        if (movementDirection.magnitude > 0) // If moving, simulate crawling
        {
            headBobTimer += Time.deltaTime * headBobFrequency;
            float bobOffset = Mathf.Sin(headBobTimer) * headBobAmount;

            // Apply head bobbing along the Y-axis
            headBobOffset = new Vector3(0, bobOffset, 0);
        }
        else
        {
            // Reset head bobbing when stationary
            headBobOffset = Vector3.zero;
            headBobTimer = 0f;
        }

        // Apply the head bob offset to the camera
        transform.position += headBobOffset;
    }
}
