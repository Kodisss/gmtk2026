using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Card Info")]
    public string cardName;

    [TextArea(3, 5)]
    public string description;

    public Sprite icon;


    [Header("Durability")]
    public int startingDurability = 3;


    [Header("Effects")]
    public CardEffect[] effects;
}