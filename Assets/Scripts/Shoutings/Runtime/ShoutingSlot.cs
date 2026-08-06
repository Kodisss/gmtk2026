using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShoutingSlot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private TMP_Text textBubble;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        textBubble = GetComponentInChildren<TMP_Text>();

        ClearMyself();
    }

    private void ClearMyself()
    {
        textBubble.text = "";
        spriteRenderer.sprite = null;
    }


    public void SetNPC(ShoutingNPC npc)
    {
        spriteRenderer.sprite = npc.portrait;

        animator.runtimeAnimatorController = npc.animator;

        animator.Play(0);

        textBubble.text = "";
    }

    public void Say(string text)
    {
        textBubble.text = text;
    }
}
