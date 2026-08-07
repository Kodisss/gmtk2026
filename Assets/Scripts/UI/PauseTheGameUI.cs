using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseTheGameUI : MonoBehaviour
{
    [SerializeField] private InputActionReference onPause;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private InputActionAsset playerControl;

    private void OnEnable()
    {
        onPause.action.performed += TogglePause;
        onPause.action.Enable();
    }

    private void OnDisable()
    {
        onPause.action.performed -= TogglePause;
        onPause.action.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        bool isPaused = !pauseUI.activeSelf;
        pauseUI.SetActive(isPaused);

        if (isPaused) playerControl.FindActionMap("Player").Disable();
        else playerControl.FindActionMap("Player").Enable();
    }

    public void BackToMenu()
    {
        MusicManager.Instance.StopMusic();
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
