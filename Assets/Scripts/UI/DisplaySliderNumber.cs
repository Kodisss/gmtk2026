using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DisplaySliderNumber : MonoBehaviour
{
    private TMP_Text numberDisplay;
    private Slider numberSlider;

    private void Awake()
    {
        numberSlider = GetComponent<Slider>();
        numberDisplay = GetComponentInChildren<TMP_Text>();
    }

    public void ChangeNumber()
    {
        numberDisplay.text = numberSlider.value.ToString("F2");
    }
}