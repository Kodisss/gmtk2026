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

        public DialogueNode GetDialogue(int index)
        {
            if (index < 0 || index >= allDialogues.Length)
            {
                Debug.LogError("Dialogue index invalid");
                return null;
            }


            return allDialogues[index];
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