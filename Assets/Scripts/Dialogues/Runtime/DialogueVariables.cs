using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueVariables : MonoBehaviour
    {
        private Dictionary<DialogueVariable, int> variables;


        private void Awake()
        {
            variables = new Dictionary<DialogueVariable, int>();

            foreach (DialogueVariable variable in 
                     System.Enum.GetValues(typeof(DialogueVariable)))
            {
                variables.Add(variable, 0);
            }
        }


        public int Get(DialogueVariable variable)
        {
            return variables[variable];
        }


        public void Modify(DialogueEffect effect)
        {
            switch (effect.operation)
            {
                case VariableOperation.Set:
                    variables[effect.variable] = effect.value;
                    break;


                case VariableOperation.Add:
                    variables[effect.variable] += effect.value;
                    break;


                case VariableOperation.Subtract:
                    variables[effect.variable] -= effect.value;
                    break;
            }
        }


        public bool Check(DialogueCondition condition)
        {
            int currentValue = Get(condition.variable);

            return condition.condition switch
            {
                ConditionOperator.Equal =>
                    currentValue == condition.value,

                ConditionOperator.Greater =>
                    currentValue > condition.value,

                ConditionOperator.Less =>
                    currentValue < condition.value,

                ConditionOperator.GreaterOrEqual =>
                    currentValue >= condition.value,

                ConditionOperator.LessOrEqual =>
                    currentValue <= condition.value,

                ConditionOperator.NotEqual =>
                    currentValue != condition.value,

                _ => false
            };
        }
    }
}