using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CharacterStats player;

    [SerializeField] private List<Image> hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Start()
    {
        player.OnHealthChanged += UpdateHearts;

        UpdateHearts(player.CurrentHealth, player.MaxHealth);
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = i < current ? fullHeart : emptyHeart;
        }
    }
}