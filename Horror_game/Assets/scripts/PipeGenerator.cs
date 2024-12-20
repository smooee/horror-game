using UnityEngine;

public class PipeGenerator : MonoBehaviour
{
    public GameObject[] pipePrefabs; // Array of pipe prefabs (straight, 90-degree, T-shape)
    public int pipeCount = 20;       // Number of pipes to generate

    private Vector3 currentPosition = Vector3.zero;  // Start position
    private Vector3 currentDirection = Vector3.forward; // Start direction

    void Start()
    {
        GeneratePipes();
    }

    void GeneratePipes()
    {
        for (int i = 0; i < pipeCount; i++)
        {
            // Choose a random pipe prefab
            GameObject selectedPipe = pipePrefabs[Random.Range(0, pipePrefabs.Length)];

            // Instantiate the pipe
            GameObject pipe = Instantiate(selectedPipe, currentPosition, Quaternion.identity);

            // Rotate the pipe to align with the current direction
            AlignPipe(pipe);

            // Update the current position and direction for the next pipe
            UpdatePositionAndDirection(pipe);
        }
    }

    void AlignPipe(GameObject pipe)
    {
        // Find the "Start" and "End" connection points
        Transform start = pipe.transform.Find("Start");
        Transform end = pipe.transform.Find("End");

        if (start == null || end == null)
        {
            Debug.LogError("Pipe prefab must have 'Start' and 'End' transforms!");
            return;
        }

        // Align the pipe's "Start" point to the current position
        pipe.transform.position = currentPosition;
        pipe.transform.rotation = Quaternion.LookRotation(currentDirection);

        // Adjust the pipe's rotation so its "Start" aligns with the current direction
        Vector3 forward = end.position - start.position; // Local forward direction of the pipe
        float angle = Vector3.SignedAngle(forward, currentDirection, Vector3.up);
        pipe.transform.Rotate(0, angle, 0, Space.World);
    }

    void UpdatePositionAndDirection(GameObject pipe)
    {
        // Find the "End" connection point
        Transform end = pipe.transform.Find("End");

        if (end == null)
        {
            Debug.LogError("Pipe prefab must have an 'End' transform!");
            return;
        }

        // Update the current position and direction
        currentPosition = end.position;
        currentDirection = pipe.transform.forward;
    }
}
