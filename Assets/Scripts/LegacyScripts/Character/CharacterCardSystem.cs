using System.Collections.Generic;
using UnityEngine;

public class CharacterCardSystem : MonoBehaviour
{
    [Header("Starting Cards")]
    [SerializeField]
    private Card[] startingCards;

    private readonly List<CardInstance> cards = new();

    private CardContext context;

    public IReadOnlyList<CardInstance> Cards => cards;

    private void Awake()
    {
        SetupContext();
        AddStartingCards();
    }

    private void SetupContext()
    {
        context = new CardContext();
        context.cardSystem = this;
        context.playerTransform = transform;
        context.movement = GetComponent<CharacterMovement2D>();
        context.health = GetComponent<CharacterHealth>();
        context.combat = GetComponent<CharacterCombat>();
        context.animator = GetComponent<Animator>();
        context.stats = GetComponent<CharacterStats>();
    }

    private void AddStartingCards()
    {
        foreach(Card card in startingCards)
        {
            AddCard(card);
        }
    }

    public void AddCard(Card card)
    {
        if(cards.Count >= 3)
        {
            Debug.Log("Card slots full");
            return;
        }

        cards.Add(
            new CardInstance(card)
        );
    }

    public void PlayCard(int index)
    {
        if(index < 0 || index >= cards.Count)
            return;

        CardInstance instance = cards[index];

        foreach(CardEffect effect in instance.Card.effects)
        {
            effect.Execute(context);
        }

        instance.Consume();

        if(instance.IsDestroyed())
        {
            cards.RemoveAt(index);
        }
    }

    public void RemoveCard(int index)
    {
        if(index < 0 || index >= cards.Count)
            return;

        cards.RemoveAt(index);
    }
}