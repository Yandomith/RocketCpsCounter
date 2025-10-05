using UnityEngine;

public class RocketDialog : MonoBehaviour
{
    [Header("Rocket Settings")]
    public Rigidbody2D rb;                // Assign your 2D rocket Rigidbody
    private float launchForceMultiplier = 2f; // Multiplier for how high the rocket flies
    public ClickCounter clickCounter;     // Reference to your ClickCounter to get highest CPS

    [SerializeField]
    private SpriteRenderer rocketFire;
    [SerializeField]
    private SpriteRenderer rocketFall;

    private bool hasLaunched = false;
    private bool rocketFallingHandled = false; // Add this flag at the class level

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rocketFire.enabled = false;
        rocketFall.enabled = false;

    }
    private void Update()
    {
        if (IsRocketFalling())
        {
            if (!rocketFallingHandled)
            {
                rocketFallingHandled = true;
                rocketFire.enabled = false;
                rocketFall.enabled = true;
            }
            wiggleRocket(); // Call every frame while falling
        }
        else
        {
            rocketFallingHandled = false; // Reset flag when not falling
        }
    }



    // ReSharper disable Unity.PerformanceAnalysis
    public void LaunchRocket()
    {
        if (hasLaunched) return; // prevent multiple launches
        hasLaunched = true;


        rocketFire.enabled = true;


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
            ResetRocket();



        }
    }

    private bool IsRocketFalling()
    {
        if (rb.linearVelocity.y < 0 && hasLaunched)
        {
            return true;
        }
        return false;
    }

    public void ResetRocket()
    {
        hasLaunched = false;
        rocketFire.enabled = false;
        rocketFall.enabled = false;
        rb.linearVelocity = Vector2.zero;  // stop movement
        rb.angularVelocity = 0f;
        rb.position = new Vector3(0, 0, -1);
        rb.rotation = 0f;
        GameManager.instance.timerText.text = "Again Click to reset?"; ;
        GameManager.instance.resetBtn.gameObject.SetActive(true);
    }


    void wiggleRocket()
    {
        // Make the rocket rotate back and forth around its Z axis for a more visible wiggle
        float wiggleAmount = 10f; // Degrees to rotate left/right
        float wiggleSpeed = 10f;  // How fast the wiggle oscillates
        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        rb.MoveRotation(wiggle);
    }
}