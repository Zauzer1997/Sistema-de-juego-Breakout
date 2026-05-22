using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;

    [Header("References")]
    [SerializeField] private Ball_Controller ball;
    [SerializeField] private ScoreAPI scoreAPI;

    [Header("Gameplay")]
    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;
    [SerializeField] private int extraLifeEvery = 10000;

    private int nextExtraLife;
    private bool isGameOver = false;

    public int Score => score;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        nextExtraLife = extraLifeEvery;

        UpdateScoreUI();
        UpdateLivesUI();

        gameOverUI.SetActive(false);
    }

    void Update()
    {
        // 👉 Reiniciar al menú con ESPACIO
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceToTop10();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;

        UpdateScoreUI();
        CheckExtraLife();
    }

    private void CheckExtraLife()
    {
        if (score >= nextExtraLife)
        {
            lives++;
            nextExtraLife += extraLifeEvery;

            UpdateLivesUI();
        }
    }

    public void LoseLife()
    {
        if (isGameOver) return;

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

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        StartCoroutine(SendScoreToAPI());
    }

    private IEnumerator SendScoreToAPI()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "PLAYER");

        yield return scoreAPI.SaveScore(playerName, score);
    }

    private void AdvanceToTop10()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Top10");
    }
}