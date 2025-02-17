using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioClip testAudio; // Assign your audio clip here
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (testAudio != null)
        {
            Debug.Log("Audio clip assigned: " + testAudio.name);
            audioSource.clip = testAudio;
            audioSource.Play(); // Play the audio on start
        }
        else
        {
            Debug.LogError("No audio clip assigned to testAudio!");
        }
    }
}
