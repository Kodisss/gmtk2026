using UnityEngine;

namespace Game.Dialogue
{
    [CreateAssetMenu(
        fileName = "Dialogue Database",
        menuName = "Dialogue/Dialogue Database")]
    public class DialogueDatabase : ScriptableObject
    {
        public DialogueNode startingDialogue;


        public DialogueNode[] allDialogues;
    }
}