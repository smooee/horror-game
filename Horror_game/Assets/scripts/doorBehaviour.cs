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

        // Start opening or closing the door
        StartCoroutine(MoveDoor(isOpen ? closedRotation : openRotation, isOpen ? closeDuration : openDuration, isOpen ? shutSound : openSound, isOpen ? null : creakSound));
        isOpen = !isOpen;
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
