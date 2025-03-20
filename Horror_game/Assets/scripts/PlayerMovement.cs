using UnityEngine;
using TMPro;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Walking speed
    public float sprintMultiplier = 2f; // Sprint speed multiplier
    public float sensitivity = 2f; // Mouse sensitivity

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    public Transform cameraTransform; // Player's camera

    // Footstep audio
    public AudioClip grassFootsteps;  // Footstep audio for grass
    public AudioClip hardwoodFootsteps;  // Footstep audio for hardwood
    private AudioSource audioSource;

    // Layer masks for surface detection
    public LayerMask grassLayer;
    public LayerMask hardwoodLayer;

    // Raycast settings
    public float raycastDistance = 5f; // Maximum distance for the raycast
    public TMP_Text interactionText;
    public TMP_Text StoryText;
    private bool visText = true;

    private bool hasBattery = false; // Tracks if the player has picked up the battery

    public GameObject monsterObject; // The monster GameObject
    public Animator monsterAnimator; // The Animator for the monster
    public AudioSource electricBoxAudio;

    public AudioSource radioAudio;

    private bool canLockDoor = false; // 🚨 NEW: This controls when locking is allowed
    private bool doorLocked = false;

    public bool windowScare = false;

    public bool powerBoxBroken = false;
    public GameObject sparksEffect;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for immersive experience
    }

    void Update()
    {
        HandleInteractionRaycast();

        // Handle movement and mouse look
        HandleMovement();
        HandleMouseLook();

        // Interact with doors
        if (Input.GetKeyDown(KeyCode.E)) InteractWithDoor();


         // Debug Key: Press "L" to trigger the monster
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(TriggerMonsterRun());
        }    


        // Handle footstep audio
        HandleFootstepAudio(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0, Input.GetKey(KeyCode.LeftShift));
    }

void HandleInteractionRaycast()
{
    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, raycastDistance))
    {
        if (hit.collider.CompareTag("Radio"))
        {
            if (visText) 
            {
                interactionText.text = "Press [E] to turn on radio";
            }

            if (Input.GetKeyDown(KeyCode.E) && visText)
            {
                if (hasBattery)
                {
                    StoryText.text = ""; // Clear previous story text
                    radioAudio.Play();
                    //StartCoroutine(TriggerMonsterRun());
                    //StartCoroutine(DelayedFlicker());

                    // 🚨 Unlock door locking feature once the radio is finished playing
                    StartCoroutine(EnableDoorLockingAfterRadio());
                }
                else
                {
                    StoryText.text = "Missing battery. Search the shed for a battery."; 
                }
                interactionText.text = "";
                visText = false;
            }
        }

        else if (hit.collider.CompareTag("Battery")) // Battery Interaction
        {
            interactionText.text = "Press [E] to pick up battery";

            if (Input.GetKeyDown(KeyCode.E))
            {
                StoryText.text = "Fix the Radio";
                visText = true; 
                hasBattery = true; 
                interactionText.text = ""; 
                hit.collider.gameObject.SetActive(false); 
            }
        }
        
        else if (hit.collider.CompareTag("PowerBox") && powerBoxBroken) // Fixing the power box
        {
            interactionText.text = "Press [E] to fix the power box";

            if (Input.GetKeyDown(KeyCode.E))
            {
                powerBoxBroken = false;
                StoryText.text = "Power restored!";
                interactionText.text = "";

                // 🚨 Deactivate sparks effect
                if (sparksEffect != null)
                {
                    sparksEffect.SetActive(false);
                }
            }
        }


        else if (hit.collider.CompareTag("door"))
        {
            doorBehaviour door = hit.collider.GetComponentInParent<doorBehaviour>();

            if (door != null)
            {
                // Normal door interaction
                interactionText.text = "Press [E] to open the door";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!doorLocked) // Only open if not locked
                    {
                        door.ToggleDoor();
                    }
                    else
                    {
                        StoryText.text = "The door is locked!";
                    }
                }

                // 🚨 Locking the door option (only before power goes out)
                if (canLockDoor && !door.isOpen && !doorLocked)
                {
                    interactionText.text += "\nPress [F] to LOCK the door";

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        door.ToggleLock();
                        doorLocked = true;
                        interactionText.text = "";
                        StoryText.text = "Get ready for bed"; // Next objective
                        windowScare = true;
                    }
                }

                // 🚨 Unlock the door ONLY AFTER power is fixed
                if (powerBoxBroken && doorLocked)
                {
                    interactionText.text += "\nPress [F] to unlock the door";

                    if (Input.GetKeyDown(KeyCode.f))
                    {
                        doorLocked = false;
                        StoryText.text = "The door is now unlocked.";
                    }
                }
            }
        }

        else
        {
            interactionText.text = "";
        }
    }
    else
    {
        interactionText.text = "";
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if(windowScare)
        {
            StartCoroutine(TriggerMonsterRun());
            StartCoroutine(DelayedFlicker());
            windowScare = false;
        }
    }

// 🚨 New Coroutine: Unlocks door locking after the radio plays
IEnumerator EnableDoorLockingAfterRadio()
{
    yield return new WaitForSeconds(radioAudio.clip.length); // Wait for the radio to finish
    canLockDoor = true;
    StoryText.text = "Lock the door"; // Prompt player to lock the door
}

    IEnumerator LockDoorsTask()
    {
        yield return new WaitForSeconds(2f); // Small delay for realism
        StoryText.text = "Lock the doors before going to bed";
    }

    IEnumerator TriggerMonsterRun()
    {
        yield return new WaitForSeconds(0.5f); // Wait 2 seconds

        monsterObject.SetActive(true); // Make monster visible
        monsterAnimator.SetTrigger("RunAcross"); // Play running animation
        monsterObject.GetComponent<MonsterMovement>().StartRunning(); // Start movement
    }

    
    IEnumerator DelayedFlicker()
    {
        yield return new WaitForSeconds(7f); // Wait 3 seconds

        // Play electrical surge sound from the electric box
        if (electricBoxAudio != null)
        {
            electricBoxAudio.Play();
        }

        yield return new WaitForSeconds(5f); // Wait a bit for sound to play before flickering

        StoryText.text = "Turn on flash light [T]";

        sparksEffect.SetActive(true);

        powerBoxBroken = true;


        // Start flickering lights
        FindObjectOfType<LightsFlicker>().StartFlickerSequence();
    }


    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && z > 0;
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (!controller.isGrounded) velocity.y += Physics.gravity.y * Time.deltaTime;
        else velocity.y = 0;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleFootstepAudio(bool isMoving, bool isSprinting)
    {
        if (isMoving)
        {
            // Detect the surface the player is on
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                if (IsInLayerMask(hit.collider.gameObject, grassLayer))
                {
                    // If the audio clip is not already set to grass footsteps, change it
                    if (audioSource.clip != grassFootsteps)
                    {
                        audioSource.clip = grassFootsteps;
                        audioSource.Play(); // Restart the audio
                    }
                }
                else if (IsInLayerMask(hit.collider.gameObject, hardwoodLayer))
                {
                    // If the audio clip is not already set to hardwood footsteps, change it
                    if (audioSource.clip != hardwoodFootsteps)
                    {
                        audioSource.clip = hardwoodFootsteps;
                        audioSource.Play(); // Restart the audio
                    }
                }

                // Adjust pitch for sprinting
                audioSource.pitch = isSprinting ? 1.8f : 1.2f;
            }

            // If the audio is not already playing, start it
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop the audio when the player stops moving
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void InteractWithDoor()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("door"))
            {
                doorBehaviour door = hit.collider.GetComponentInParent<doorBehaviour>();
                door?.ToggleDoor();
            }
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
}
