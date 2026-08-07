using UnityEngine;
using UnityEngine.SceneManagement;

public class TItleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingScreen;

    public void StartTheGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        settingScreen.SetActive(false);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
