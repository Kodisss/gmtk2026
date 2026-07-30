using UnityEngine;
using UnityEngine.SceneManagement;

public class TItleScreenManager : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
