using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ReputationUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text reputationText;
    private int previousReputation = 0;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        int nextReputation = gameManager.Reputation;

        if (nextReputation != previousReputation)
        {
            UpdateReputation(nextReputation);
            previousReputation = nextReputation;
        }
    }


    private void UpdateReputation(int value)
    {
        reputationText.text = $"Reputation: {value}";
    }
}