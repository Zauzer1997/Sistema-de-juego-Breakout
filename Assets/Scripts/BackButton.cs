using UnityEngine;
using UnityEngine.SceneManagement;

public class Top10UI : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}