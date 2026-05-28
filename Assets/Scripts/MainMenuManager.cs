using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_Text highscoreText;

    private string highscoreURL = "http://localhost:5000/api/scores/highscore";

    void Start()
    {
        StartCoroutine(RefreshFlow());
    }
    private IEnumerator RefreshFlow()
    {
        yield return new WaitForSeconds(0.2f);
        yield return GetHighscore();
    }

    public void PlayGame()
    {
        string playerName = playerNameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "PLAYER";

        PlayerPrefs.SetString("PlayerName", playerName);

        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator GetHighscore()
    {
        UnityWebRequest request = UnityWebRequest.Get(highscoreURL);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            HighscoreData data =
                JsonUtility.FromJson<HighscoreData>(request.downloadHandler.text);

            if (data != null)
            {
                highscoreText.text =
                    $"Highscore: {data.playerName} - {data.scoreValue}";
            }
        }
        else
        {
            highscoreText.text = "Highscore: ERROR";
            Debug.LogError(request.error);
        }
    }
}