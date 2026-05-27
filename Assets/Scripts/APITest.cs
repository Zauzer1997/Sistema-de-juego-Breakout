using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APITest : MonoBehaviour
{
    IEnumerator Start()
    {
        string url = "http://127.0.0.1:5299/api/scores/top10";

        Debug.Log("Calling API...");

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        Debug.Log("Result: " + request.result);
        Debug.Log("Error: " + request.error);
        Debug.Log("Response: " + request.downloadHandler.text);
    }
}