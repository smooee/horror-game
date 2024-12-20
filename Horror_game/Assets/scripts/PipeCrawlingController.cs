using UnityEngine;

public class PipeCrawlingController : MonoBehaviour
{
    public float crawlSpeed = 2f;        // Speed of crawling movement
    public float headBobFrequency = 2f; // How fast the head bobs
    public float headBobAmount = 0.05f; // How much the head bobs up/down
    public float headTiltAmount = 2f;   // How much the camera tilts when bobbing

    private Transform cameraTransform;  // Reference to the child camera for head bobbing
    private float headBobTimer = 0f;    // Timer for bobbing effect
    private float currentXRotation = 0f; // Store the camera's current pitch (vertical rotation)

    void Start()
    {
        // Get the camera child transform
        cameraTransform = Camera.main.transform;

        // Lock the cursor for a first-person experience
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();  // Move the player object
        HandleHeadBob();   // Bob the camera independently
        HandleLooking();   // Allow the player to look around
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W)) // Move forward
        {
            // This ensures no movement along X or Y axes
            Vector3 forwardMovement = transform.forward * crawlSpeed * Time.deltaTime;
            forwardMovement.y = 0f; // Lock the Y movement so the player doesn’t go up/down
            forwardMovement.x = 0f; // Lock the X movement so the player stays on the Z-axis
            transform.Translate(forwardMovement, Space.World); // Move in world space
        }
    }

    void HandleHeadBob()
    {
        if (Input.GetKey(KeyCode.W)) // Head bob only when moving forward
        {
            // Increment head bob timer based on movement
            headBobTimer += Time.deltaTime * headBobFrequency;

            // Calculate vertical bob offset using a sine wave
            float verticalBob = Mathf.Sin(headBobTimer) * headBobAmount;

            // Apply head bobbing to the child camera's local position (only Y-axis)
            cameraTransform.localPosition = new Vector3(0, verticalBob, 0);
        }
        else
        {
            // Reset bobbing when not moving
            headBobTimer = 0f;
            cameraTransform.localPosition = Vector3.zero;
        }
    }

    void HandleLooking()
    {
        // Get mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the parent object (yaw) for horizontal movement
        transform.Rotate(Vector3.up, mouseX);

        // Rotate the child camera (pitch) for vertical movement
        currentXRotation -= mouseY; // Pitch the camera up/down with the mouse
        currentXRotation = Mathf.Clamp(currentXRotation, -80f, 80f); // Limit vertical rotation

        // Apply the vertical rotation to the camera's local rotation
        cameraTransform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
    }
}
