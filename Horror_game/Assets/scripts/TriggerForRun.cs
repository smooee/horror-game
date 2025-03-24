using UnityEngine;
using System.Collections;


public class TriggerForRun : MonoBehaviour
{
    public GameObject monsterObject; // The monster GameObject
    public Transform runStartPoint;  // Starting position of the monster
    public Transform runEndPoint;    // Ending position of the monster
    public float runSpeed = 10f;     // Speed of the run


    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (PlayerMovement.powerFixed && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(MonsterRun());
            Debug.Log("Monster run triggered!");
        }
    }

    IEnumerator MonsterRun()
    {
        // Set monster to start position and make it visible
        monsterObject.SetActive(true);
        monsterObject.transform.position = runStartPoint.position;

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
        monsterObject.SetActive(false);
    }
}
