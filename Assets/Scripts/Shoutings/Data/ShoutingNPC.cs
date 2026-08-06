using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New NPC",
    menuName = "Dialogue/NPC")]
public class ShoutingNPC : ScriptableObject
{
    public string npcName;

    public Sprite portrait;

    public AnimatorController animator;

    public AudioClip voice;
}