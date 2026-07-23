using UnityEngine;
using UnityEngine.Events;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueChoice
    {
        [Header("Choice Text")]
        public string text;


        [Header("Next Dialogue")]
        public DialogueNode nextDialogue;


        [Header("Requirements")]
        public DialogueCondition[] conditions;


        [Header("Effects")]
        public DialogueEffect[] effects;


        [Header("Events")]
        public UnityEvent onSelected;
    }
}