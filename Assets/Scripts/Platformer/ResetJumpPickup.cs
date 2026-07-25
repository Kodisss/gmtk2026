using UnityEngine;

public class ResetJumpPickup : MonoBehaviour
{
    private CharacterStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        stats = FindAnyObjectByType<CharacterStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stats.NewDashPickup();
        Destroy(gameObject);
    }
}
