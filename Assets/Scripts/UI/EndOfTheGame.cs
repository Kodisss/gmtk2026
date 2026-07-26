using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndOfTheGame : MonoBehaviour
{
    private MusicManager musicManager;

    [SerializeField] private GameObject myUI;
    [SerializeField] private float lostTimePerFrames;
    [SerializeField] private Sprite[] lostTheGameSprite;
    [SerializeField] private float winTimePerFrames;
    [SerializeField] private Sprite[] wonTheGameSprite;
    [SerializeField] private Image displayImage;
    [SerializeField] private MusicTrack lostTheGameTrack;
    [SerializeField] private MusicTrack wonTheGameTrack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        musicManager = MusicManager.Instance;
        myUI.SetActive(false);
    }

    public void EndOfGame(int reputationScore)
    {
        myUI.SetActive(true);

        if(reputationScore >= 3)
        {
            StartCoroutine(AnimateTheImages(winTimePerFrames, wonTheGameSprite));
            musicManager.FadeToMusic(wonTheGameTrack);
        }
        else
        {
            StartCoroutine(AnimateTheImages(lostTimePerFrames, lostTheGameSprite));
            musicManager.FadeToMusic(lostTheGameTrack);
        }
    }

    private IEnumerator AnimateTheImages(float framerate, Sprite[] images)
    {
        int imageCount = 0;
        int maxImage = images.Length;

        while (true)
        {
            displayImage.sprite = images[imageCount];

            yield return new WaitForSeconds(framerate);

            imageCount++;
            if(imageCount >= images.Length) imageCount = 0;
        }
        
    }
}
