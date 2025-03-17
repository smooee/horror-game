using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight; // Drag your Flashlight GameObject here
    public Transform cameraTransform; // Assign the Player's Camera

    public float horizontalFollowSpeed = 8f; // Delay for left/right movement
    public float verticalFollowSpeed = 6f; // Delay for up/down movement

    private bool isOn = false; // Keeps track of flashlight state
    private Quaternion currentRotation;

    void Start()
    {
        if (flashlight != null)
            flashlight.SetActive(false); // Ensure flashlight is off at the start

        currentRotation = flashlight.transform.rotation;
    }

    void Update()
    {
        // Toggle flashlight with 'T'
        if (Input.GetKeyDown(KeyCode.T))
        {
            isOn = !isOn;
            flashlight.SetActive(isOn);
        }

        if (flashlight.activeSelf) // Only update if flashlight is on
        {
            SmoothFollow();
        }
    }

    void SmoothFollow()
    {
        // Get the camera's target rotation
        Quaternion targetRotation = cameraTransform.rotation;

        // Extract horizontal and vertical rotation separately
        Vector3 currentEuler = currentRotation.eulerAngles;
        Vector3 targetEuler = targetRotation.eulerAngles;

        // Smoothly interpolate horizontal (Y) and vertical (X) separately
        float smoothY = Mathf.LerpAngle(currentEuler.y, targetEuler.y, horizontalFollowSpeed * Time.deltaTime);
        float smoothX = Mathf.LerpAngle(currentEuler.x, targetEuler.x, verticalFollowSpeed * Time.deltaTime);

        // Apply new rotation while keeping Z the same
        currentRotation = Quaternion.Euler(smoothX, smoothY, currentEuler.z);
        flashlight.transform.rotation = currentRotation;
    }
}
