using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public enum DialogueState
        {
            Hidden,
            Typing,
            WaitingForContinue,
            WaitingForChoice
        }

        [Header("References")]
        [SerializeField] private DialogueUI ui;
        [SerializeField] private DialogueTypewriter typewriter;
        [SerializeField] private DialogueVariables variables;
        [Header("Settings")]
        [SerializeField] private bool startHidden = true;

        private DialogueNode currentNode;

        private int currentLineIndex;

        private DialogueState state = DialogueState.Hidden;

        public bool WaitingForYesNo => state == DialogueState.WaitingForChoice && currentNode != null && currentNode.dialogueType == DialogueType.YesNo;
        public DialogueState CurrentState => state;

        private void Start()
        {
            if (startHidden) ui.Hide();
        }

        #region Dialogue Starting

        public void StartDialogue(DialogueNode node)
        {
            if (node == null)
            {
                Debug.LogWarning("Tried starting a null dialogue node.");

                return;
            }

            currentNode = node;

            currentLineIndex = 0;

            ApplyEffects(node.onEnterEffects);

            node.onEnter?.Invoke();

            ui.Show();

            ui.ResetUI();

            ui.SetSpeaker(node.speakerName);

            DisplayCurrentLine();
        }

        public void EndDialogue()
        {
            DialogueNode finishedNode = currentNode;

            if (currentNode != null)
            {
                currentNode.onExit?.Invoke();
            }

            if (finishedNode != null && finishedNode.dialogueType == DialogueType.End)
            {
                EndDialogueRoutineInspection();
                GameManager.Instance.DialogueFinished(finishedNode);
            }

            EndDialogueRoutineInspection();
        }

        private void EndDialogueRoutineInspection()
        {
            currentNode = null;

            currentLineIndex = 0;

            state = DialogueState.Hidden;

            ui.ResetUI();
            ui.Hide();
        }

        #endregion

        #region Display

        private void DisplayCurrentLine()
        {
            if (currentNode == null)
                return;


            if (currentLineIndex >= currentNode.lines.Length)
            {
                HandleNodeEnd();
                return;
            }

            DialogueLine line = currentNode.lines[currentLineIndex];

            ui.SetPortrait(line.portrait, line.PortraitAnimationFPS);

            state = DialogueState.Typing;

            typewriter.Type(
                ui.DialogueText,
                line.text,
                line.voiceClip,
                line.voiceVolume,
                line.typingSpeed,
                OnTypingFinished);
        }

        private void HandleNodeEnd()
        {
            switch (currentNode.dialogueType)
            {
                case DialogueType.End:
                    EndDialogue();
                    break;

                default:
                    DisplayChoices();
                    break;
            }
        }

        private void OnTypingFinished()
        {
            state =
                DialogueState.WaitingForContinue;
        }

        #endregion

        #region Input

        public void Continue()
        {
            switch (state)
            {
                case DialogueState.Typing:

                    typewriter.FinishImmediately();

                    state =
                        DialogueState.WaitingForContinue;

                    break;



                case DialogueState.WaitingForContinue:

                    currentLineIndex++;

                    DisplayCurrentLine();

                    break;
            }
        }

        public void Interrupt()
        {
            if (state != DialogueState.Typing) return;

            if (currentNode.interruptDialogue == null) return;

            StartDialogue(currentNode.interruptDialogue);
        }


        #endregion

        #region Choices

        private void DisplayChoices()
        {
            ui.ClearChoices();

            ui.SetPortrait(currentNode.ChoicePortraits, currentNode.ChoicePortraitAnimationFPS);

            switch (currentNode.dialogueType)
            {
                case DialogueType.Normal:

                    foreach (DialogueChoice choice in currentNode.choices)
                    {
                        CheckConditions(choice);
                    }

                    break;



                case DialogueType.YesNo:
                    ui.DisplayYesNoText();

                    break;
            }

            state = DialogueState.WaitingForChoice;
        }

        private void Choose(DialogueChoice choice)
        {
            if (choice == null)
                return;

            ApplyEffects(choice.effects);


            choice.onSelected?.Invoke();

            StartDialogue(choice.nextDialogue);
        }

        #endregion

        #region Conditions / Effects


        private void CheckConditions(DialogueChoice choice)
        {
            if (choice.conditions == null)
            {
                ui.CreateChoice(choice.text, () => Choose(choice), true);
                return;
            }

            DialogueCondition thatDidntWork;
            bool shouldItBeOn = true;
            string endDialogue = choice.text;

            foreach (DialogueCondition condition in choice.conditions)
            {
                if (!variables.Check(condition))
                {
                    thatDidntWork = condition;
                    shouldItBeOn = false;
                    string conditionBetterWording;


                    switch (condition.condition)
                    {
                        case ConditionOperator.Equal:
                            conditionBetterWording = "not equal to ";
                            break;
                        case ConditionOperator.NotEqual:
                            conditionBetterWording = "anything but ";
                            break;
                        case ConditionOperator.Greater:
                            conditionBetterWording = "greater than ";
                            break;
                        case ConditionOperator.Less:
                            conditionBetterWording = "lower than ";
                            break;
                        case ConditionOperator.GreaterOrEqual:
                            conditionBetterWording = "greater than ";
                            break;
                        case ConditionOperator.LessOrEqual:
                            conditionBetterWording = "greater than ";
                            break;
                        default:
                            conditionBetterWording = " ";
                            break;
                    }

                    endDialogue = "(" + condition.variable + " must be " + conditionBetterWording + condition.value + ") " + endDialogue;
                }  
            }

            ui.CreateChoice(endDialogue, () => Choose(choice), shouldItBeOn);
        }



        private void ApplyEffects(DialogueEffect[] effects)
        {
            if (effects == null) return;

            foreach (DialogueEffect effect in effects)
            {
                variables.Modify(effect);
            }
        }


        #endregion

        public void Yes()
        {
            if (!WaitingForYesNo)
                return;

            Choose(currentNode.yesChoice);
        }



        public void No()
        {
            if (!WaitingForYesNo)
                return;

            Choose(currentNode.noChoice);
        }
    }
}