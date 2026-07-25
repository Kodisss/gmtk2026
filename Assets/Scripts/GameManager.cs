using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [SerializeField]
    private int daysLeft = 7;

    [SerializeField]
    private int reputation = 0;


    public event System.Action<int> OnDaysChanged;
    public event System.Action<int> OnReputationChanged;

    public int DaysLeft
    {
        get => daysLeft;

        set
        {
            if (daysLeft == value)
                return;

            daysLeft = value;

            OnDaysChanged?.Invoke(daysLeft);
        }
    }

    public int Reputation
    {
        get => reputation;

        set
        {
            int clampedValue = Mathf.Clamp(value, -5, 5);

            if (reputation == clampedValue)
                return;

            reputation = clampedValue;

            OnReputationChanged?.Invoke(reputation);
        }
    }

    [Header("Death")]
    [SerializeField]
    private float restartDelay = 2f;
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



    private void Start()
    {
        MusicManager.Instance.PlayMusic("Day3");
    }

    public void PlayerDied()
    {
        if (restarting)
            return;

        restarting = true;

        StartCoroutine(RestartSceneRoutine());
    }



    private IEnumerator RestartSceneRoutine()
    {
        yield return new WaitForSeconds(restartDelay);


        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);


        restarting = false;
    }
}