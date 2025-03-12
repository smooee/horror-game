using UnityEngine;

public class doorBehaviour : MonoBehaviour
{
    public float openYRotation = -20f;
    public float closedYRotation = 90f;
    public float openDuration = 2f;
    public float closeDuration = 0.5f;
    public AudioClip openSound;
    public AudioClip creakSound;
    public AudioClip shutSound;

    private bool isOpen = false;
    private bool isMoving = false;

    public bool reverseSwing = false; // Set this to true in the Inspector for doors that should open the other way

    private Quaternion openRotation;
    private Quaternion closedRotation;
    private AudioSource audioSource;

    void Start()
    {
        closedRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, closedYRotation, transform.rotation.eulerAngles.z);
        openRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, openYRotation, transform.rotation.eulerAngles.z);
        
        // Use or add an AudioSource component to the door
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    public void ToggleDoor()
{
    if (isMoving) return;

    Quaternion targetRotation = isOpen ? closedRotation : 
        (reverseSwing ? Quaternion.Euler(transform.rotation.eulerAngles.x, -openYRotation, transform.rotation.eulerAngles.z) : openRotation);

    StartCoroutine(MoveDoor(targetRotation, isOpen ? closeDuration : openDuration, isOpen ? shutSound : openSound, isOpen ? null : creakSound));

    if (!isOpen) // If the door is opening, start auto-close timer
    {
        //StartCoroutine(AutoCloseDoor());
    }

    isOpen = !isOpen;
}

// Coroutine to close the door automatically after 1.5 seconds
private System.Collections.IEnumerator AutoCloseDoor()
{
    yield return new WaitForSeconds(openDuration + 1.5f); // Ensures door fully opens before countdown

    if (isOpen && !isMoving) // Only close if door is still open
    {
        ToggleDoor(); // Close the door
    }
}


    private System.Collections.IEnumerator MoveDoor(Quaternion targetRotation, float duration, AudioClip startSound, AudioClip extraSound = null)
    {
        isMoving = true;

        // Play the initial sound (open or shut)
        if (startSound != null)
        {
            audioSource.clip = startSound;
            audioSource.Play();
        }

        // Optionally play an additional creaking sound
        if (extraSound != null)
        {
            yield return new WaitForSeconds(0.2f);
            audioSource.PlayOneShot(extraSound);
        }

        float timeElapsed = 0f;
        Quaternion initialRotation = transform.rotation;

        // Smoothly rotate the door over time
        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to the final rotation to ensure precision
        transform.rotation = targetRotation;
        isMoving = false;
    }
}
