using UnityEngine;

public class DoubleJumpBoostPickup : MonoBehaviour
{
    CharacterStats characterStats;

    [SerializeField] private float boostDuration = 5f;

    private void Start()
    {
        characterStats = FindAnyObjectByType<CharacterStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.EnableDoubleJump(boostDuration);
        Destroy(gameObject);
    }
}
