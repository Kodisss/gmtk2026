using TMPro;
using UnityEngine;

public class DaysLeftUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text daysText;


    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnDaysChanged += UpdateDays;

            UpdateDays(GameManager.Instance.DaysLeft);
        }
    }


    private void OnDisable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnDaysChanged -= UpdateDays;
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