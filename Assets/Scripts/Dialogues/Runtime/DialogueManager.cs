using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField]
        private DialogueUI ui;

        [SerializeField]
        private DialogueTypewriter typewriter;

        [SerializeField]
        private DialogueVariables variables;


        [Header("Settings")]
        [SerializeField]
        private bool startHidden = true;


        private DialogueNode currentNode;

        private int currentLineIndex;

        private DialogueState state =
            DialogueState.Hidden;



        public DialogueState CurrentState => state;



        private void Awake()
        {
            if (startHidden)
                ui.Hide();
        }



        #region Dialogue Starting


        public void StartDialogue(DialogueNode node)
        {
            if (node == null)
            {
                Debug.LogWarning(
                    "Tried starting a null dialogue node.");

                return;
            }


            currentNode = node;

            currentLineIndex = 0;


            ApplyEffects(
                node.onEnterEffects);


            node.onEnter?.Invoke();


            ui.Show();

            ui.ResetUI();

            ui.SetSpeaker(
                node.speakerName);


            DisplayCurrentLine();
        }



        public void EndDialogue()
        {
            if (currentNode != null)
            {
                currentNode.onExit?.Invoke();
            }


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
                DisplayChoices();

                return;
            }


            DialogueLine line =
                currentNode.lines[currentLineIndex];


            ui.SetPortrait(
                line.portrait);


            state =
                DialogueState.Typing;


            typewriter.Type(
                ui.DialogueText,
                line.text,
                OnTypingFinished);
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
            if (state != DialogueState.Typing)
                return;


            if (currentNode.interruptDialogue == null)
                return;


            StartDialogue(
                currentNode.interruptDialogue);
        }


        #endregion



        #region Choices


        private void DisplayChoices()
        {
            ui.ClearChoices();


            switch (currentNode.dialogueType)
            {
                case DialogueType.Normal:

                    foreach (DialogueChoice choice
                            in currentNode.choices)
                    {
                        if (CheckConditions(choice))
                        {
                            DialogueChoice selected =
                                choice;

                            ui.CreateChoice(
                                selected.text,
                                () =>
                                Choose(selected));
                        }
                    }

                    break;



                case DialogueType.YesNo:


                    ui.CreateChoice(
                        "YES",
                        () =>
                        Choose(
                            currentNode.yesChoice));


                    ui.CreateChoice(
                        "NO",
                        () =>
                        Choose(
                            currentNode.noChoice));

                    break;
            }


            state =
                DialogueState.WaitingForChoice;
        }



        private void Choose(DialogueChoice choice)
        {
            if (choice == null)
                return;


            ApplyEffects(
                choice.effects);


            choice.onSelected?.Invoke();


            StartDialogue(
                choice.nextDialogue);
        }



        #endregion



        #region Conditions / Effects


        private bool CheckConditions(
            DialogueChoice choice)
        {
            if (choice.conditions == null)
                return true;


            foreach (DialogueCondition condition
                    in choice.conditions)
            {
                if (!variables.Check(condition))
                    return false;
            }


            return true;
        }



        private void ApplyEffects(
            DialogueEffect[] effects)
        {
            if (effects == null)
                return;


            foreach (DialogueEffect effect in effects)
            {
                variables.Modify(effect);
            }
        }


        #endregion
    }
}