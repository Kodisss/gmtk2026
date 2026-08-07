using UnityEngine;
using UnityEngine.SceneManagement;

public class TItleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingScreen;

    private GameSettings gameSettings;

    private void Start()
    {
        gameSettings = GameSettings.Instance;
    }

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

    public void ModifySFXVolume(float value)
    {
        gameSettings.SoundVolume = value;
    }
    public void ModifyMusicVolume(float value)
    {
        gameSettings.MusicVolume = value;
    }
    public void ModifyVoiceVolume(float value)
    {
        gameSettings.VoiceVolume = value;
    }
}
