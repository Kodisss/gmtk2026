using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    [Header("Damage")]
    public int damage = 10;


    [Header("Attack Shape")]
    public GameObject hitboxPrefab;

    public float attackOffset = 1f;


    [Header("Knockback")]
    public float knockbackForce = 5f;


    [Header("Timing")]
    public float attackDuration = 0.2f;


    [Header("Animation")]
    public string animationTrigger;


    [Header("Audio")]
    public AudioClip sound;
}