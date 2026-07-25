using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CharacterStats player;

    [SerializeField] private HeartUILogic heartPrefab;
    [SerializeField] private Transform heartHolder;
    [SerializeField] private List<HeartUILogic> hearts;

    private void Start()
    {
        player.OnHealthChanged += UpdateHearts;

        InitiateHearts();
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= UpdateHearts;
    }

    private void InitiateHearts()
    {
        for (int i = 0; i < player.MaxHealth; i++)
        {
            HeartUILogic currentHeart = Instantiate(heartPrefab, heartHolder);

            currentHeart.InitiateHeart();

            hearts.Add(currentHeart);
        }
    }

    private void UpdateHearts(int current)
    {
        for (int i = 0; i < hearts.Count; i++) 
        {
            if (i > current - 1) hearts[i].KillYourself();
        }
    }
}