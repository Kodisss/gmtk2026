using UnityEngine;

public class CardContext
{
    public CharacterCardSystem cardSystem;
    public CharacterMovement2D movement;
    public CharacterHealth health;
    public CharacterCombat combat;
    public CharacterStats stats;
    public Animator animator;
    public Transform playerTransform;

    public Vector2 FacingDirection
    {
        get
        {
            if (playerTransform.localScale.x >= 0) return Vector2.right;
            return Vector2.left;
        }
    }
}