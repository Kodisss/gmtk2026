using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueVariables : MonoBehaviour
    {

        public int Get(DialogueVariable variable)
        {
            if (GameManager.Instance == null)
            {
                Debug.LogWarning(
                    "GameManager missing while reading dialogue variable.");

                return 0;
            }


            return variable switch
            {
                DialogueVariable.DaysLeft =>
                    GameManager.Instance.DaysLeft,


                DialogueVariable.Reputation =>
                    GameManager.Instance.Reputation,


                _ => 0
            };
        }



        public void Modify(DialogueEffect effect)
        {
            switch (effect.variable)
            {
                case DialogueVariable.DaysLeft:

                    GameManager.Instance.DaysLeft =
                        ApplyOperation(
                            GameManager.Instance.DaysLeft,
                            effect);

                    break;


                case DialogueVariable.Reputation:

                    GameManager.Instance.Reputation =
                        ApplyOperation(
                            GameManager.Instance.Reputation,
                            effect);

                    break;
            }
        }

        private int ApplyOperation(int variable, DialogueEffect effect)
        {
            switch (effect.operation)
            {
                case VariableOperation.Set:
                    return effect.value;

                case VariableOperation.Add:
                    return variable + effect.value;

                case VariableOperation.Subtract:
                    return variable - effect.value;
            }

            return variable;
        }

        public bool Check(DialogueCondition condition)
        {
            int currentValue =
                Get(condition.variable);

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