using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverUI;

    private bool isGameover = false;
    void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        if (isGameover) { return; }

        isGameover = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(isGameover && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
