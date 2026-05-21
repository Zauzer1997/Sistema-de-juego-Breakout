using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;

    private float horizontalInput;
    [SerializeField]private float limiteX = 3.20f;
    [SerializeField] private float speed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + Vector2.right * horizontalInput * speed  * Time.fixedDeltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -limiteX, limiteX);

        rb.MovePosition(targetPosition);
    }
}
