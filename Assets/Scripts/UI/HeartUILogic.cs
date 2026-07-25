using UnityEngine;
using UnityEngine.UI;

public class HeartUILogic : MonoBehaviour
{
    private Image myHeart;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite deadHeart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        myHeart = GetComponent<Image>();
    }

    public void InitiateHeart()
    {
        myHeart.sprite = fullHeart;
    }

    public void KillYourself()
    {
        myHeart.sprite = deadHeart;
    }
}
