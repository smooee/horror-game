using System.Collections;
using UnityEngine;

public class LightsFlicker : MonoBehaviour
{
    public GameObject lightObject; // The light GameObject to turn on/off
    public float flickerDuration = 5f; // How long the flickering lasts before full blackout
    public float minFlickerTime = 0.1f; // Minimum time light stays on/off
    public float maxFlickerTime = 0.5f; // Maximum time light stays on/off
    public AudioClip flickerSound; // Optional: Assign a flickering sound for extra effect

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (flickerSound != null && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource if needed
        }
        //StartFlickerSequence();
    }

    public void StartFlickerSequence()
    {
        StartCoroutine(FlickerLights());
    }

    private IEnumerator FlickerLights()
    {
        float endTime = Time.time + flickerDuration; // Set when the flickering should end

        while (Time.time < endTime)
        {
            bool lightsOn = Random.value > 0.5f; // Randomly decide if lights should be on or off

            if (lightObject != null)
            {
                lightObject.SetActive(lightsOn); // Enable/Disable light GameObject
            }

            if (flickerSound != null && lightsOn) // Play sound when flickering ON
            {
                audioSource.PlayOneShot(flickerSound);
            }

            // Reduce flicker time as we approach blackout for a "power dying" effect
            float flickerTime = Mathf.Lerp(maxFlickerTime, minFlickerTime, 1 - ((endTime - Time.time) / flickerDuration));
            yield return new WaitForSeconds(flickerTime);
        }

        // Full blackout: Ensure all lights are OFF at the end
        if (lightObject != null)
        {
            lightObject.SetActive(false);
        }
    }
}
