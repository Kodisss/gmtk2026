using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Dialogue;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState gameState;

    [Header("Boss Dialogue")]
    [SerializeField] private DialogueDatabase bossDialogueDatabase;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private CharacterMovement2D playerMovement;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private EndOfDialogueSceneReset endOfDialogueSceneReset;
    [SerializeField] private EndOfTheGame endOfGame;

    [SerializeField] private int daysLeft;
    [SerializeField] private int reputation;

    public int DaysLeft
    {
        get => daysLeft;

        set
        {
            if (daysLeft == value)
                return;

            daysLeft = value;
            gameState.DaysLeft = daysLeft;

            if (daysLeft == 0)
                Debug.Log("World should end");
        }
    }


    public int Reputation
    {
        get => reputation;

        set
        {
            reputation = Mathf.Clamp(value, -5, 5);
            gameState.Reputation = reputation;
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
    }

    private void Start()
    {
        gameState = GameState.Instance;

        reputation = gameState.Reputation;
        daysLeft = gameState.DaysLeft;

        MusicManager.Instance.PlayTrackNb(daysLeft);
    }

    public void StartBossDialogue()
    {
        if (playerMovement == null)
        {
            Debug.LogError("Player not registered");
            return;
        }


        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not registered");
            return;
        }


        playerMovement.SetMovementEnabled(false);

        if(playerMovement.GetComponent<Animator>() != null) playerMovement.GetComponent<Animator>().Play("Idle");

        dialogueManager.StartDialogue(bossDialogueDatabase.GetDialogue(gameState.CurrentBossDialogue));
    }

    public void DialogueFinished(DialogueNode node)
    {
        Debug.Log("Dialogue Finished triggered in GameManager");

        if (node.dialogueType != DialogueType.End)
            return;


        if (node.advanceDialogueDatabase)
        {
            bossDialogueDatabase.Advance();
        }


        playerMovement.SetMovementEnabled(true);


        if (endOfDialogueSceneReset != null)
        {
            gameState.CurrentBossDialogue++;
            DaysLeft = daysLeft - 1;
            endOfDialogueSceneReset.GoToNextDay();
        }
    }

    public void PlayerDied()
    {
        if (restarting)
            return;

        restarting = true;

        StartCoroutine(RestartSceneRoutine());
    }

    public void RestartScene()
    {
        if(daysLeft > 0) StartCoroutine(RestartSceneRoutine());
        else if (daysLeft == 0) endOfGame.EndOfGame(reputation);
    }

    private IEnumerator RestartSceneRoutine()
    {
        yield return new WaitForSeconds(restartDelay);


        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);


        restarting = false;
    }
}