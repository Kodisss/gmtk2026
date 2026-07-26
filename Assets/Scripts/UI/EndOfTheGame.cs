using UnityEngine;
using UnityEngine.UI;

public class EndOfTheGame : MonoBehaviour
{
    [SerializeField] private GameObject myUI;
    [SerializeField] private Sprite lostTheGameSprite;
    [SerializeField] private Sprite wonTheGameSprite;
    [SerializeField] private Image displayImage;
    private MusicManager musicManager;
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
            displayImage.sprite = wonTheGameSprite;
            musicManager.FadeToMusic(wonTheGameTrack);
        }
        else
        {
            displayImage.sprite = lostTheGameSprite;
            musicManager.FadeToMusic(lostTheGameTrack);
        }
    }
}
