using UnityEngine;

public class CardPickup : MonoBehaviour
{
    private Card card;



    public void Initialize(Card card)
    {
        this.card = card;


        SpriteRenderer renderer =
            GetComponent<SpriteRenderer>();


        if(renderer != null)
        {
            renderer.sprite = card.icon;
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterCardSystem inventory =
            other.GetComponent<CharacterCardSystem>();


        if(inventory == null)
            return;



        inventory.AddCard(card);


        Destroy(gameObject);
    }
}