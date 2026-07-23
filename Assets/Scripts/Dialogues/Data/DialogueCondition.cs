using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueCondition
    {
        [Header("Variable")]
        public DialogueVariable variable;


        [Header("Comparison")]
        public ConditionOperator condition;


        [Header("Value")]
        public int value;
    }
}