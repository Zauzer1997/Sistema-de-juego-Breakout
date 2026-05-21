using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private int maxHealth = 4;
    private int currentHealth;

    [SerializeField] private Color red = Color.red;
    [SerializeField] private Color yellow = Color.yellow;
    [SerializeField] private Color green = Color.green;
    [SerializeField] private Color blue = Color.blue;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateColor();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damange)
    {
        GameManager.Instance.AddScore(GetHitScore());

        currentHealth -= damange;
        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(GetDestroyScore());

            Destroy(gameObject);
            return;
        }

        UpdateColor();
    }

    private int GetHitScore()
    {
        return maxHealth * 10;
    }
    private int GetDestroyScore()
    {
        return maxHealth * 50;
    }

    private void UpdateColor()
    {
        switch (currentHealth)
        {
            case 4:
                spriteRenderer.color = red;
                break;
            case 3:
                spriteRenderer.color = yellow;
                break;
            case 2:
                spriteRenderer.color = green;
                break;
            case 1:
                spriteRenderer.color = blue;
                break;
        }
    }
}