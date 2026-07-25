using UnityEngine;

public class DashAvaiableManager : MonoBehaviour
{
    private CharacterMovement2D movements;
    [SerializeField] private SpriteRenderer canDashSpriteRenderer;
    [SerializeField] private Sprite canDashSprite;

    private void Start()
    {
        movements = GetComponentInParent<CharacterMovement2D>();
        canDashSpriteRenderer.sprite = null;
    }

    private void Update()
    {
        if(!movements.CanDash) canDashSpriteRenderer.sprite = null;
        else canDashSpriteRenderer.sprite = canDashSprite;
    }
}
