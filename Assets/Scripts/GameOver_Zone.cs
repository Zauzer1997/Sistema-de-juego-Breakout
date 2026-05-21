using UnityEngine;

public class GameOver_Zone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Game Over!");
        }
    }
}
