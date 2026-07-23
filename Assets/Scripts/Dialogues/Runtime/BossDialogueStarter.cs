using UnityEngine;

namespace Game.Dialogue
{
    public class BossDialogueStarter : MonoBehaviour
    {
        [SerializeField]
        private DialogueNode startingDialogue;


        private void Start()
        {
            FindAnyObjectByType<DialogueManager>().StartDialogue(startingDialogue);
        }
    }
}