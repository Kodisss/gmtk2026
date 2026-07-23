using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueEffect
    {
        [Header("Variable")]
        public DialogueVariable variable;


        [Header("Operation")]
        public VariableOperation operation;


        [Header("Amount")]
        public int value;
    }
}