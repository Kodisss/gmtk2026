using TMPro;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text reputationText;


    private void OnEnable()
    {
        GameManager.Instance.OnReputationChanged += UpdateReputation;
        UpdateReputation(GameManager.Instance.Reputation);
    }


    private void OnDisable()
    {
        GameManager.Instance.OnReputationChanged -= UpdateReputation;
    }


    private void UpdateReputation(int value)
    {
        reputationText.text = $"Reputation: {value}";
    }
}