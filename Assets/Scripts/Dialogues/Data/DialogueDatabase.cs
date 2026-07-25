using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueDatabase : MonoBehaviour
    {
        public DialogueNode[] allDialogues;
        public int currentDialogueIndex = 0;


        public DialogueNode GetCurrentDialogue()
        {
            if (allDialogues.Length == 0)
                return null;


            return allDialogues[currentDialogueIndex];
        }


        public void Advance()
        {
            currentDialogueIndex++;


            if (currentDialogueIndex >= allDialogues.Length)
            {
                currentDialogueIndex = allDialogues.Length - 1;
            }
        }
    }
}