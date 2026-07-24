using System;
using UnityEngine;

[Serializable]
public class CardInstance
{
    public Card Card { get; private set; }

    public int Durability { get; private set; }

    public string Name => Card.cardName;

    public Sprite Icon => Card.icon;
    
    public string Description => Card.description;


    public CardInstance(Card card)
    {
        Card = card;
        Durability = card.startingDurability;
    }


    public void Consume()
    {
        Durability--;
    }


    public bool IsDestroyed()
    {
        return Durability <= 0;
    }
}