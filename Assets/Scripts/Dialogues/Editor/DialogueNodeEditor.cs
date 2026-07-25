#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Game.Dialogue.Editor
{
    [CustomEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : UnityEditor.Editor
    {
        private DialogueNode node;


        private void OnEnable()
        {
            node = (DialogueNode)target;
        }



        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            DrawHeader();

            EditorGUILayout.Space();


            DrawProperty("speakerName");

            DrawProperty("dialogueType");


            EditorGUILayout.Space();


            DrawLines();


            EditorGUILayout.Space();


            DrawDialogueTypeSpecific();


            EditorGUILayout.Space();


            DrawAdvanced();


            serializedObject.ApplyModifiedProperties();
        }



        private void DrawLines()
        {
            EditorGUILayout.LabelField(
                "Dialogue Lines",
                EditorStyles.boldLabel);


            DrawProperty("lines");
        }



        private void DrawDialogueTypeSpecific()
        {
            switch (node.dialogueType)
            {
                case DialogueType.Normal:

                    DrawNormalChoices();

                    break;


                case DialogueType.YesNo:

                    DrawYesNoChoices();

                    break;


                case DialogueType.End:

                    DrawEndDialogue();

                    break;
            }
        }



        private void DrawNormalChoices()
        {
            EditorGUILayout.LabelField(
                "Choice Display",
                EditorStyles.boldLabel);


            DrawProperty("ChoicePortraits");

            DrawProperty(
                "ChoicePortraitAnimationFPS");


            DrawProperty("choices");
        }



        private void DrawYesNoChoices()
        {
            EditorGUILayout.LabelField(
                "YES / NO",
                EditorStyles.boldLabel);


            DrawYesNoChoice(
                "yesChoice",
                "YES");


            DrawYesNoChoice(
                "noChoice",
                "NO");
        }



        private void DrawEndDialogue()
        {
            EditorGUILayout.HelpBox(
                "End dialogue: only lines are displayed. Dialogue closes automatically afterwards.",
                MessageType.Info);
        }



        private void DrawYesNoChoice(
            string propertyName,
            string label)
        {
            SerializedProperty choice =
                serializedObject.FindProperty(propertyName);


            if (choice == null)
                return;


            SerializedProperty nextDialogue =
                choice.FindPropertyRelative(
                    "nextDialogue");


            EditorGUILayout.BeginVertical(
                EditorStyles.helpBox);


            EditorGUILayout.LabelField(
                label,
                EditorStyles.boldLabel);


            EditorGUILayout.PropertyField(
                nextDialogue);


            EditorGUILayout.EndVertical();
        }



        private void DrawAdvanced()
        {
            EditorGUILayout.LabelField(
                "Advanced",
                EditorStyles.boldLabel);


            DrawProperty("interruptDialogue");

            DrawProperty("onEnterEffects");

            DrawProperty("onEnter");

            DrawProperty("onExit");
        }



        private void DrawProperty(string propertyName)
        {
            SerializedProperty property =
                serializedObject.FindProperty(propertyName);


            if (property != null)
            {
                EditorGUILayout.PropertyField(
                    property,
                    true);
            }
        }



        private void DrawHeader()
        {
            EditorGUILayout.LabelField(
                "Dialogue Node",
                EditorStyles.boldLabel);


            EditorGUILayout.HelpBox(
                GetHelpText(),
                MessageType.Info);
        }



        private string GetHelpText()
        {
            return node.dialogueType switch
            {
                DialogueType.Normal =>
                    "Normal dialogue: lines followed by choices.",


                DialogueType.YesNo =>
                    "Yes/No dialogue: player answers by nodding or shaking.",


                DialogueType.End =>
                    "End dialogue: displays lines and closes the conversation.",


                _ =>
                    ""
            };
        }
    }
}

#endif