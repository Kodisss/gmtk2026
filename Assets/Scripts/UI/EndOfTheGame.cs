using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndOfTheGame : MonoBehaviour
{
    private MusicManager musicManager;

    [SerializeField] private GameObject myUIwon;
    [SerializeField] private GameObject myUIlost;
    [SerializeField] private float winTimePerFrames;
    [SerializeField] private Sprite[] wonTheGameSprite;
    [SerializeField] private Image displayImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private MusicTrack lostTheGameTrack;
    [SerializeField] private MusicTrack wonTheGameTrack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        musicManager = MusicManager.Instance;
        myUIwon.SetActive(false);
        myUIlost.SetActive(false);
    }

    public void EndOfGame(int reputationScore)
    {
        if(reputationScore >= 3)
        {
            myUIwon.SetActive(true);
            StartCoroutine(AnimateTheImages(winTimePerFrames, wonTheGameSprite));
            musicManager.FadeToMusic(wonTheGameTrack);
        }
        else
        {
            myUIlost.SetActive(true);
            videoPlayer.Play();
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
