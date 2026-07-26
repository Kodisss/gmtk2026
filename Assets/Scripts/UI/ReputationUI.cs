using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
    [SerializeField] private TMP_Text reputationText;
    private int previousReputation = 0;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        HideMyself();
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

    public void HideMyself()
    {
        reputationText.gameObject.SetActive(false);
    }

    public void ShowMyself()
    {
        reputationText.gameObject.SetActive(true);
    }


    private void UpdateReputation(int value)
    {
        reputationText.text = $"Reputation: {value}";
    }
}