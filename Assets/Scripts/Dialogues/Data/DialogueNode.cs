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

        [Header("End Dialogue")]
        [Tooltip("Used only by End dialogue. Advances the dialogue database.")]
        public bool advanceDialogueDatabase = true;

        [Header("Lines")]
        public DialogueLine[] lines;

        [Tooltip("Used for our character face when the choice button appear")]
        public Sprite[] ChoicePortraits;
        [Range(1f, 10f)]
        public int ChoicePortraitAnimationFPS = 8;

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

        private void OnValidate()
        {
            if (lines == null)
                return;

            foreach (DialogueLine line in lines)
            {
                if (line == null)
                    continue;


                if (line.PortraitAnimationFPS <= 0)
                    line.PortraitAnimationFPS = 8;


                if (line.typingSpeed <= 0)
                    line.typingSpeed = 1f;


                if (line.voiceVolume <= 0)
                    line.voiceVolume = 1f;
            }
        }
    }
}