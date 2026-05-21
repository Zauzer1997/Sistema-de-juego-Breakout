using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Ball_Controller ball;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int score = 0;
    [SerializeField] private int extraLifeEvery = 10000;
    private int nextExtraLife;

    [SerializeField] private int lives = 3;

    private bool isGameover = false;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        nextExtraLife = extraLifeEvery;
        UpdateLivesUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        CheckExtraLife();
    }


    private void CheckExtraLife()
    {
        if(score >= nextExtraLife)
        {
            lives++;
            UpdateLivesUI();
            Debug.Log("EXTRA LIFE!!!");
            nextExtraLife += extraLifeEvery;
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score:" + score;
    }

    public void GameOver()
    {
        if (isGameover) { return; }

        isGameover = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoseLife()
    {
        if (isGameover) { return; }
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            ball.ResetBall();
        }

    }

    public void UpdateLivesUI()
    {
        livesText.text = "Lives: " + lives;
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
