using System.Collections;
using TMPro;
using UnityEngine;

public class EndOfDialogueSceneReset : MonoBehaviour
{
    [SerializeField] private GameObject myUI;
    [SerializeField] private GameObject wholeUI;
    private GameManager gameManager;
    private MusicManager musicManager;
    [SerializeField] private TMP_Text textDisplay;
    [SerializeField] private AudioClip endOfDaySound;

    private void Start()
    {
        gameManager = GameManager.Instance;
        musicManager = MusicManager.Instance;

        myUI.SetActive(false);
    }

    public void GoToNextDay()
    {
        wholeUI.SetActive(false);

        StartCoroutine(NextDayRoutine());
    }

    private IEnumerator NextDayRoutine()
    {
        // Show UI
        myUI.SetActive(true);
        musicManager.StopMusic();


        // Reset text alpha
        Color textColor = textDisplay.color;
        textColor.a = 0f;
        textDisplay.color = textColor;

        if(gameManager.DaysLeft > 1)
        {
            textDisplay.text = gameManager.DaysLeft + " days left...";
        }
        else
        {
            textDisplay.text = gameManager.DaysLeft + " day left...";
        }

        yield return new WaitForSeconds(.5f);

        // Play sound
        if (endOfDaySound != null)
        {
            musicManager.PlaySomething(endOfDaySound);
        }

        // Fade in text
        float fadeDuration = 1f;
        float timer = 0f;


        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = timer / fadeDuration;

            textColor.a = alpha;
            textDisplay.color = textColor;

            yield return null;
        }


        // Make sure it ends fully visible
        textColor.a = 1f;
        textDisplay.color = textColor;


        // Wait before changing day
        yield return new WaitForSeconds(3f);


        // Restart scene / next day
        gameManager.RestartScene();
    }
}
