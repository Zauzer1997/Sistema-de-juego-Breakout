using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ball_Controller : MonoBehaviour
{
    private Rigidbody2D ballRb;
    private bool launched;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minHorizontalSpeed = 0.8f;
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


        PreventStuckMovement();
    }

    private void PreventStuckMovement()
    {
        Vector2 velocity = ballRb.linearVelocity;

        
        if (Mathf.Abs(velocity.x) < minHorizontalSpeed)
        {
            float randomX = Random.Range(-1f, 1f);

            velocity = new Vector2(
                randomX,
                velocity.y
            ).normalized * speed;
        }

       
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
