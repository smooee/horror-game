using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform startPoint;
    public Transform endPoint;
    private bool isRunning = false;

    public AudioClip monsterSound;
    private AudioSource audioSource;

    void Start()
    {
        transform.position = startPoint.position;
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                StartCoroutine(DisableAfterSound()); // Wait for sound before disabling
                isRunning = false;
            }
        }
    }

    public void StartRunning()
    {
        isRunning = true;

        if (audioSource != null && monsterSound != null)
        {
            audioSource.PlayOneShot(monsterSound);
        }
    }

    IEnumerator DisableAfterSound()
    {
        // Wait until the sound finishes playing
        if (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(monsterSound.length);
        }

        gameObject.SetActive(false); // Disable the monster **after** the sound ends
    }
}
