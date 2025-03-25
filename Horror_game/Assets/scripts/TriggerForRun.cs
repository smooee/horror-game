using UnityEngine;
using System.Collections;

public class TriggerForRun : MonoBehaviour
{
    public GameObject monsterObject;          // The monster GameObject
    public Transform runStartPoint;           // Starting position of the monster
    public Transform runEndPoint;             // Ending position of the monster
    public float runSpeed = 10f;              // Speed of the run

    public AudioSource jumpScareSource;       // Sudden jumpscare sound
    public GameObject mainMusicObject;        // GameObject with calm music
    public GameObject intenseMusicObject;     // GameObject with intense music

    public PlayerMovement playerMovement;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {

        if (PlayerMovement.powerFixed && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(MonsterRun());
        }
    }

    IEnumerator MonsterRun()
    {
        // Set monster to start position and make it visible
        monsterObject.SetActive(true);
        monsterObject.transform.position = runStartPoint.position;

        // Play jumpscare audio
        jumpScareSource.Play();

        // 🚨 Switch background music
        SwitchMusic();


        // Move monster towards the end point
        while (Vector3.Distance(monsterObject.transform.position, runEndPoint.position) > 0.1f)
        {
            monsterObject.transform.position = Vector3.MoveTowards(
                monsterObject.transform.position,
                runEndPoint.position,
                runSpeed * Time.deltaTime
            );
            yield return null;
        }

        // (Optional) Disable monster after reaching the endpoint
        yield return new WaitForSeconds(1f);

        PlayerMovement.canLockDoor = true;  // Enable door locking again
        playerMovement.StoryText.text = "Lock yourself in the house!"; // New prompt
        PlayerMovement.toldToHide = true;

        monsterObject.SetActive(false);
    }

    void SwitchMusic()
    {
        // 🚨 Deactivate the main music
        if (mainMusicObject != null)
        {
            mainMusicObject.SetActive(false);
        }

        // 🚨 Activate the intense music
        if (intenseMusicObject != null)
        {
            intenseMusicObject.SetActive(true);
        }
    }
}
