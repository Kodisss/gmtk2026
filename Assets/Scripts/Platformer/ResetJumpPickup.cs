using UnityEngine;

public class ResetJumpPickup : MonoBehaviour
{
    private CharacterMovement2D movements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        movements = FindAnyObjectByType<CharacterMovement2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        movements.CanDash = true;
        Destroy(gameObject);
    }
}
