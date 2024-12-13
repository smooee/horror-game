using UnityEngine;

public class PipeCrawlController : MonoBehaviour
{
    public Animator animator; // Assign the Animator component in the Inspector
    public float normalSpeed = 1f; // Normal crawl animation speed
    public float fastSpeed = 2f; // Fast crawl animation speed
    public float moveSpeed = 2f; // Movement speed for normal crawling
    public float fastMoveSpeed = 4f; // Movement speed for fast crawling

    private void Update()
    {
        float speed = 0f; // Default speed is 0 (no movement)
        float animationSpeed = normalSpeed; // Default animation speed

        // Check for input
        if (Input.GetKey(KeyCode.W))
        {
            // Determine if Shift is held
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = fastMoveSpeed; // Move faster
                animationSpeed = fastSpeed; // Play animation faster
            }
            else
            {
                speed = moveSpeed; // Normal movement speed
                animationSpeed = normalSpeed; // Normal animation speed
            }

            // Move the character forward
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Update the animation speed in the Animator
        animator.SetFloat("Speed", animationSpeed);
    }
}
