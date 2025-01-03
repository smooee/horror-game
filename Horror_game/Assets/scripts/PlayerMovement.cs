using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float sensitivity = 2f; // Mouse sensitivity

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    public Transform cameraTransform; // Player's camera
    public float bobAmount = 0.05f; // Camera bobbing height
    public float bobSpeed = 6f; // Camera bobbing speed
    private float bobTimer = 0f;

    // Footstep audio
    public AudioClip footstepAudio; // Assign the 52-second MP3 file here
    public float stepDuration = 0.5f; // Time between steps (adjust to walking pace)
    private float footstepTimer = 0f;
    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for immersive experience
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        if (!controller.isGrounded)
            velocity.y += Physics.gravity.y * Time.deltaTime;
        else
            velocity.y = 0;
        controller.Move(velocity * Time.deltaTime);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Footstep handling
        PlayFootstep();
    }

    void PlayFootstep()
    {
        if (controller.isGrounded && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            footstepTimer += Time.deltaTime;
            bobTimer += Time.deltaTime * bobSpeed; // Increment bob timer

            // Apply bobbing to camera
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
            cameraTransform.localPosition = new Vector3(0, 1 + bobOffset, 0);

            if (footstepTimer >= stepDuration)
            {
                // Play a random section of the MP3
                audioSource.time = Random.Range(0, footstepAudio.length - 1f); // Randomize start time
                audioSource.PlayOneShot(footstepAudio, 0.7f); // Slightly lower volume
                footstepTimer = 0f;
            }
        }
        else
        {
            // Reset bobbing when not moving
            cameraTransform.localPosition = new Vector3(0, 1, 0);
            bobTimer = 0f;
        }
    }
}
