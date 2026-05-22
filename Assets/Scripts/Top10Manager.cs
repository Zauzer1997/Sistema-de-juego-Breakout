using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class Top10Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoresText;

    private string url = "http://localhost:5000/api/scores/top10";

    void Start()
    {
        StartCoroutine(GetTop10());
    }

    private IEnumerator GetTop10()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            HighscoreData[] scores = JsonHelper.FromJson<HighscoreData>(json);

            ShowScores(scores);
        }
        else
        {
            scoresText.text = "ERROR LOADING SCORES";
            Debug.LogError(request.error);
        }
    }

    private void ShowScores(HighscoreData[] scores)
    {
        string result = "TOP 10\n\n";

        for (int i = 0; i < scores.Length; i++)
        {
            result += $"{i + 1}. {scores[i].playerName} - {scores[i].scoreValue}\n";
        }

        scoresText.text = result;
    }
}