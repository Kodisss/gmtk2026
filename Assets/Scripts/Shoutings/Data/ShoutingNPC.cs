using UnityEngine;

[CreateAssetMenu(
    fileName = "New NPC",
    menuName = "Dialogue/NPC")]
public class ShoutingNPC : ScriptableObject
{
    public string npcName;

    public Sprite portrait;

    public RuntimeAnimatorController animator;

    public AudioClip voice;
}