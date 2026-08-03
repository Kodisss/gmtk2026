using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Dialogue;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState gameState;
    [SerializeField] private GameObject rainParticleSystem;

    [Header("Boss Dialogue")]
    [SerializeField] private DialogueDatabase bossDialogueDatabase;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private CharacterMovement2D playerMovement;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private BeginingOfSceneDisplay beginingOfSceneDisplay;
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

        if (daysLeft == 1) rainParticleSystem.SetActive(true);

        if(gameState.PlayIntro) beginingOfSceneDisplay.PlayIntro();
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

        gameState.CurrentBossDialogue++;
        DaysLeft = daysLeft - 1;

        RestartScene();
    }

    public void PlayerDied()
    {
        if (restarting) return;

        restarting = true;

        StartCoroutine(RestartSceneRoutine());
    }

    public void RestartScene()
    {
        playerMovement.SetMovementEnabled(false);

        if (daysLeft > 0)
        {
            gameState.PlayIntro = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (daysLeft == 0)
        {
            endOfGame.EndOfGame(reputation);
        }
    }

    private IEnumerator RestartSceneRoutine()
    {
        playerMovement.SetMovementEnabled(false);

        yield return new WaitForSeconds(restartDelay);

        gameState.PlayIntro = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        restarting = false;
    }
}