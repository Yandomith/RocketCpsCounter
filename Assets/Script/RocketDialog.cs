using UnityEngine;

public class RocketDialog : MonoBehaviour
{
    [Header("Rocket Settings")]
    public Rigidbody2D rb;                // Assign your 2D rocket Rigidbody
    private float launchForceMultiplier = 2f; // Multiplier for how high the rocket flies
    public ClickCounter clickCounter;     // Reference to your ClickCounter to get highest CPS

    private bool hasLaunched = false;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; 


    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void LaunchRocket()
    {
        if (hasLaunched) return; // prevent multiple launches
        hasLaunched = true;



        // Calculate force based on highest CPS
        float highestCPS = clickCounter ? clickCounter.GetHighestCPS() : 0f;

        // Apply an upward force
        rb.AddForce(Vector2.up * (highestCPS * launchForceMultiplier), ForceMode2D.Impulse);

        Debug.Log("Rocket Launched with CPS: " + highestCPS);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && hasLaunched)
        {
            Debug.Log("Rocket Hit the Ground!");
            hasLaunched = false;

      
            rb.linearVelocity = Vector2.zero;  // stop movement
            rb.angularVelocity = 0f;
            rb.position = new Vector3(0, 0,-1);
            GameManager.instance.timerText.text = "Again Click to reset?";;
            GameManager.instance.resetBtn.gameObject.SetActive(true);

        }
    }

}