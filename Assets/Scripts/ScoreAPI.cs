using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ScoreAPI : MonoBehaviour
{
    private const string API_URL =
        "http://127.0.0.1:5299/api/scores";

    public IEnumerator SaveScore(string playerName, int score)
    {
        HighscoreData data = new HighscoreData
        {
            playerName = playerName,
            scoreValue = score
        };

        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(
            API_URL,
            "POST"
        );

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score saved!");
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
