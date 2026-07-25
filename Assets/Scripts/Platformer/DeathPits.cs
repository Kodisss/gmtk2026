using UnityEngine;

public class DeathPits : MonoBehaviour
{
    private CharacterStats character;

    private void Start()
    {
        character = FindAnyObjectByType<CharacterStats>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterStats>() == character)
        {
            character.Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
