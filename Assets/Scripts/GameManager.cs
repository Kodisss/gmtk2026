using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Dialogue;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Boss Dialogue")]
    [SerializeField] private CharacterMovement2D playerMovement;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueDatabase bossDialogueDatabase;

    [SerializeField]
    private int daysLeft = 7;

    [SerializeField]
    private int reputation = 0;


    public int DaysLeft
    {
        get => daysLeft;

        set
        {
            if (daysLeft == value)
                return;

            daysLeft = value;

            if (daysLeft == 0) Debug.Log("World should end"); // do something
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
        dialogueManager.OnEndDialogueNode += DialogueFinished;
    }

    public void StartBossDialogue()
    {
        playerMovement.SetMovementEnabled(false);
        dialogueManager.StartDialogue(bossDialogueDatabase.GetCurrentDialogue());
    }

    private void DialogueFinished(DialogueNode node)
    {
        if (node.dialogueType != DialogueType.End)
            return;


        if (node.advanceDialogueDatabase)
        {
            bossDialogueDatabase.Advance();
        }

        playerMovement.SetMovementEnabled(true);
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