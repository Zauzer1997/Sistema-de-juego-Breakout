using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ball_Controller : MonoBehaviour
{
    private Rigidbody2D ballRb;
    private bool launched;

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Ball Settings")]
    [SerializeField] private float minHorizontalSpeed = 0.8f;
    [SerializeField] private float speed = 8f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceSFX;

    [Header("Audio Pitch")]
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Mantener pelota pegada al paddle antes de iniciar
        if (!launched)
        {
            transform.position = playerTransform.position + new Vector3(0, 0.6f, 0);
        }

        // Lanzar pelota
        if (!launched && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        launched = true;

        Vector2 direction = new Vector2(-0.7f, 0.7f).normalized;

        ballRb.linearVelocity = direction * speed;
    }

    void FixedUpdate()
    {
        if (!launched)
            return;

        // Mantener velocidad constante
        ballRb.linearVelocity = ballRb.linearVelocity.normalized * speed;

        PreventStuckMovement();
    }

    private void PreventStuckMovement()
    {
        Vector2 velocity = ballRb.linearVelocity;

        // Evitar movimiento demasiado horizontal
        if (Mathf.Abs(velocity.x) < minHorizontalSpeed)
        {
            float randomX = Random.Range(-1f, 1f);

            velocity = new Vector2(
                randomX,
                velocity.y
            ).normalized * speed;
        }

        // Evitar movimiento demasiado vertical
        if (Mathf.Abs(velocity.y) < minHorizontalSpeed)
        {
            float randomY = Random.Range(-1f, 1f);

            velocity = new Vector2(
                velocity.x,
                randomY
            ).normalized * speed;
        }

        ballRb.linearVelocity = velocity;
    }

    public void ResetBall()
    {
        launched = false;

        ballRb.linearVelocity = Vector2.zero;
        ballRb.angularVelocity = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reproducir SFX de rebote
        PlayBounceSound();

        // Rebote dinámico en paddle
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerX = collision.transform.position.x;

            float difference = transform.position.x - playerX;

            Vector2 newDirection = new Vector2(difference, 1f).normalized;

            ballRb.linearVelocity = newDirection * speed;
        }
    }

    private void PlayBounceSound()
    {
        if (audioSource == null || bounceSFX == null)
            return;

        // Pitch dinámico basado en velocidad
        float speedPitch = 1f + (ballRb.linearVelocity.magnitude * 0.015f);

        // Variación aleatoria arcade clásica
        float randomPitch = Random.Range(minPitch, maxPitch);

        audioSource.pitch = speedPitch * randomPitch;

        audioSource.PlayOneShot(bounceSFX);
    }
}
