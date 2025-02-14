using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [Header("Speed Settings")]
    public float maxSpeed = 25f;
    public float initialSpeed = 2.0f;
    public float speedMultiplier = 1.2f;
    private float currentSpeed;

    [Header("Paddle Bounce Angle Settings")]
    [Tooltip("Maximum angle (in degrees) away from horizontal for paddle bounces.")]
    public float maxBounceAngleDeg = 45f;

    [Header("Wall Bounce Settings")]
    [Tooltip("Angle (in degrees) to use for wall bounces.")]
    public float wallBounceAngleDeg = 45f;

    [Header("Audio Clips")]
    public AudioClip paddleBounceClip;
    public AudioClip wallBounceClip;
    public AudioClip goalClip;
    [Tooltip("Played on collisions if current speed >= highSpeedThreshold.")]
    public AudioClip highSpeedClip;
    public float highSpeedThreshold = 10f;

    private Rigidbody rb;
    private Renderer ballRenderer;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ballRenderer = GetComponent<Renderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the Ball! Adding one now.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        currentSpeed = initialSpeed;
        Vector3 startVelocity = new Vector3(1f, 0f, 0f).normalized * currentSpeed; //starting ball moving in positive x
        rb.linearVelocity = startVelocity;
    }

    void Update()
    {
        UpdateBallColor();
    }

    private void OnCollisionEnter(Collision other)
    {
        float speed = rb.linearVelocity.magnitude;

        if (other.gameObject.CompareTag("Paddle"))
        {
            currentSpeed = Mathf.Min(currentSpeed * speedMultiplier, maxSpeed);

            if (speed >= highSpeedThreshold && highSpeedClip != null)
            {
                audioSource.clip = highSpeedClip;
            }
            else
            {
                audioSource.clip = paddleBounceClip;
            }
            audioSource.Play();

            ContactPoint contact = other.contacts[0];
            float paddleCenterZ = other.collider.bounds.center.z;
            float paddleExtentZ = other.collider.bounds.extents.z;
            float hitFactor = (contact.point.z - paddleCenterZ) / paddleExtentZ;
            float bounceAngle = hitFactor * maxBounceAngleDeg * Mathf.Deg2Rad;
            float directionX = (other.transform.position.x < transform.position.x) ? 1f : -1f;
            Vector3 newDirection = new Vector3(directionX * Mathf.Cos(bounceAngle), 0f, Mathf.Sin(bounceAngle));
            rb.linearVelocity = newDirection.normalized * currentSpeed;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (speed >= highSpeedThreshold && highSpeedClip != null)
            {
                audioSource.clip = highSpeedClip;
            }
            else
            {
                audioSource.clip = wallBounceClip;
            }
            audioSource.Play();
        }

            Debug.Log($"currentSpeed: {currentSpeed}");
    }
    public void ResetPositionAndLaunch()
    {
        transform.position = Vector3.zero; //moving the ball to the centre

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; //zeroing out velocity
        currentSpeed = 4f;

        float startXDirection = (Random.value > 0.5f) ? 1f : -1f; //currently sending in random direction
        Vector3 startVelocity = new Vector3(startXDirection, 0f, 0f).normalized * 2f;
        rb.linearVelocity = startVelocity;
    }

    private void UpdateBallColor()
    {
        if (ballRenderer == null || ballRenderer.material == null) return;

        float speed = rb.linearVelocity.magnitude;

        // 0 = yellow, 1 = red
        float t = Mathf.InverseLerp(initialSpeed, maxSpeed, speed);

        // interpolate from yellow to red
        Color newColor = Color.Lerp(Color.yellow, Color.red, t);

        ballRenderer.material.color = newColor;
    }

}

