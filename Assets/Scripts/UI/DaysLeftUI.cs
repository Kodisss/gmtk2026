using TMPro;
using UnityEngine;

public class DaysLeftUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text daysText;
    private int previousDaysCount = 0;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        int nextDaysCount = gameManager.DaysLeft;

        if (nextDaysCount != previousDaysCount)
        {
            UpdateDays(nextDaysCount);
            previousDaysCount = nextDaysCount;
        }
    }


    private void UpdateDays(int days)
    {
        daysText.text =
            days == 1 ?
            "1 day left" :
            $"{days} days left";
    }
}