using System.Collections;
using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    private CharacterStats character;

    [SerializeField] private float timerBeforeNewDamage = 1f;
    private bool canDealDamage = true;

    private void Start()
    {
        character = FindAnyObjectByType<CharacterStats>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterStats>() == character)
        {
            if (canDealDamage)
            {
                character.TakeDamage(1);
                //canDealDamage = false;
                //StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(timerBeforeNewDamage);
        canDealDamage = true;
    }
}
