using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    CharacterStats characterStats;

    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float boostDuration = 5f;

    private AudioClip speedBoostOn;
    private AudioClip speedBoostOff;

    private void Start()
    {
        characterStats = FindAnyObjectByType<CharacterStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.ApplySpeedMultiplier(speedMultiplier, boostDuration);
        Destroy(gameObject);
    }
}
