using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float restartDelay = 2f;

    private bool restarting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayerDied()
    {
        if (restarting) return;

        restarting = true;

        StartCoroutine(RestartSceneRoutine());
    }

    private IEnumerator RestartSceneRoutine()
    {
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        restarting = false;
    }
}