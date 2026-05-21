using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ball_Controller : MonoBehaviour
{
    private Rigidbody2D ballRb;
    private bool launched;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minHorizontalSpeed = 0.5f;
    [SerializeField] private float speed = 8f;
    
    void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!launched)
        {
            transform.position = playerTransform.position + new Vector3(0, 0.6f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
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
        if (!launched) { return; }

        ballRb.linearVelocity = ballRb.linearVelocity.normalized * speed;


        FixVerticalMovement();
    }

    private void FixVerticalMovement()
    {
        if (Mathf.Abs(ballRb.linearVelocity.x) < minHorizontalSpeed)
        {
            float randomX = Random.Range(-1f, 1f);

            Vector2 fixedDirection = new Vector2(randomX, ballRb.linearVelocity.y).normalized;

            ballRb.linearVelocity = fixedDirection * speed;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float playerX = collision.transform.position.x;

            float difference = transform.position.x - playerX;

            Vector2 newDirection = new Vector2(difference, 1f).normalized;

            ballRb.linearVelocity = newDirection * speed;
        }
    }
}
