using UnityEngine;

public class PowerAudioTrigger : MonoBehaviour
{
    public GameObject audioSourceObject; // Drag your audio GameObject here
    //public PlayerMovement playerMovement; // Reference to PlayerMovement script

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (PlayerMovement.powerBoxBroken && !hasTriggered)
        {
            // 🚨 Only trigger once when power needs fixing
            audioSourceObject.SetActive(true);
            hasTriggered = true;
            Debug.Log("Power audio triggered.");
        }
    }
}
