using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShoutingSlot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private ShoutingTypewriter typewriter;
    private AudioClip npcVoice;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        typewriter = GetComponentInChildren<ShoutingTypewriter>();

        ClearMyself();
    }


    private void ClearMyself()
    {
        spriteRenderer.sprite = null;
        typewriter.Clear();
    }


    public void SetNPC(ShoutingNPC npc)
    {
        spriteRenderer.sprite = npc.portrait;

        animator.runtimeAnimatorController = npc.animator;

        animator.Play(0);

        npcVoice = npc.voice;

        typewriter.Clear();
    }

    public void Say(string text, float speed)
    {
        typewriter.Type(text, npcVoice, speed);
    }

    public bool IsTyping()
    {
        return typewriter.IsTyping;
    }

    public void ClearText()
    {
        typewriter.Clear();
    }
}