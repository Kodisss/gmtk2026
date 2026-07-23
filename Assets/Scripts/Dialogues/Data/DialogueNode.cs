using UnityEngine;
using UnityEngine.Events;

namespace Game.Dialogue
{
    [CreateAssetMenu(
        fileName = "New Dialogue Node",
        menuName = "Dialogue/Dialogue Node")]
    public class DialogueNode : ScriptableObject
    {
        [Header("Speaker")]
        public string speakerName;


        [Header("Dialogue Type")]
        public DialogueType dialogueType;


        [Header("Lines")]
        public DialogueLine[] lines;



        [Header("Normal Choices")]
        [Tooltip("Used only for Normal dialogue.")]
        public DialogueChoice[] choices;



        [Header("Yes / No")]
        [Tooltip("Used only for Yes/No dialogue.")]
        public DialogueChoice yesChoice;

        public DialogueChoice noChoice;



        [Header("Interrupt")]
        public DialogueNode interruptDialogue;



        [Header("Effects")]
        public DialogueEffect[] onEnterEffects;



        [Header("Events")]
        public UnityEvent onEnter;

        public UnityEvent onExit;
    }
}