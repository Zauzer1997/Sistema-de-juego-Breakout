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

    [Header("Start UI")]
    [SerializeField] private TMP_Text readyText;

    [Header("References")]
    [SerializeField] private Ball_Controller ball;
    [SerializeField] private ScoreAPI scoreAPI;

    [Header("Gameplay")]
    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;
    [SerializeField] private int extraLifeEvery = 10000;

    [Header("Start Jingle")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startJingle;
    [SerializeField] private Player_Controller playerController;
    [SerializeField] private Ball_Controller ballController;

    private Rigidbody2D ballRb;

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

        ballRb = ballController.GetComponent<Rigidbody2D>();

        StartCoroutine(StartGameSequence());
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceToTop10();
        }
    }

    private IEnumerator StartGameSequence()
    {
        playerController.enabled = false;

        Rigidbody2D rb = ballController.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;

        ballController.transform.position =
            playerController.transform.position + new Vector3(0f, 0.6f, 0f);

        readyText.gameObject.SetActive(true);
        readyText.text = "READY?";
        readyText.enabled = true;

        audioSource.PlayOneShot(startJingle);

        
        yield return StartCoroutine(BlinkReadyText());

        
        yield return new WaitForSeconds(0.3f);

        readyText.gameObject.SetActive(false);

        rb.simulated = true;
        playerController.enabled = true;

        ballController.ResetBall();
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

    private IEnumerator BlinkReadyText()
    {
        float blinkSpeed = 0.25f;
        float elapsed = 0f;

        while (elapsed < startJingle.length - 0.7f)
        {
            readyText.enabled = !readyText.enabled;

            yield return new WaitForSeconds(blinkSpeed);
            elapsed += blinkSpeed;
        }

        readyText.enabled = true;
        readyText.text = "START!";
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