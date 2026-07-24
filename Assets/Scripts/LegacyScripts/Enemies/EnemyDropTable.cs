using UnityEngine;

[System.Serializable]
public class CardDrop
{
    public Card card;

    [Range(0f, 100f)]
    public float dropChance;
}



public class EnemyDropTable : MonoBehaviour
{
    [SerializeField]
    private CardDrop[] possibleDrops;


    [SerializeField]
    private GameObject cardPickupPrefab;



    public void DropCard()
    {
        Card selectedCard = RollCard();


        if(selectedCard == null)
            return;


        Instantiate(
            cardPickupPrefab,
            transform.position,
            Quaternion.identity
        )
        .GetComponent<CardPickup>()
        .Initialize(selectedCard);
    }



    private Card RollCard()
    {
        float roll =
            Random.Range(0f,100f);


        float current = 0f;


        foreach(CardDrop drop in possibleDrops)
        {
            current += drop.dropChance;


            if(roll <= current)
            {
                return drop.card;
            }
        }


        return null;
    }
}