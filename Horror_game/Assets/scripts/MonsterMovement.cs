using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust speed in the Inspector
    public Transform startPoint; // The left side of the window
    public Transform endPoint;   // The right side of the window

    private bool isRunning = false;

    void Start()
    {
        transform.position = startPoint.position; // Start at the left side
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            gameObject.SetActive(true);
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, moveSpeed * Time.deltaTime);

            // Stop moving once it reaches the end
            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                gameObject.SetActive(false); // Hide the monster after crossing
            }
        }
    }

    public void StartRunning()
    {
        isRunning = true;
    }
}
