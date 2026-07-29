using System.Collections;
using UnityEngine;

public abstract class Boost : MonoBehaviour
{
    protected CharacterStats stats;
    protected SpriteRenderer sprite;
    protected Collider2D collider;

    [SerializeField] protected float respawnTimer = 5f;

    protected virtual void Start()
    {
        stats = FindAnyObjectByType<CharacterStats>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Optional: only react to the player
        if (!collision.TryGetComponent<CharacterMovement2D>(out _))
            return;

        ApplyBoost();
        StartCoroutine(BoostCollected());
    }

    protected abstract void ApplyBoost();

    private IEnumerator BoostCollected()
    {
        collider.enabled = false;
        sprite.enabled = false;

        yield return new WaitForSeconds(respawnTimer);

        collider.enabled = true;
        sprite.enabled = true;
    }
}