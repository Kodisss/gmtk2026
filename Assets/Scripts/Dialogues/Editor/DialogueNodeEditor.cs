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

            // DrawAdvanced(); // Displays onenter stuff and unity events on choice and all

            serializedObject.ApplyModifiedProperties();
        }



        private void DrawLines()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Dialogue Lines", EditorStyles.boldLabel);

            DrawProperty("lines");

            EditorGUILayout.EndVertical();
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
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Choice Display", EditorStyles.boldLabel);

            DrawProperty("ChoicePortraits");

            DrawProperty("ChoicePortraitAnimationFPS");

            DrawProperty("choices");

            EditorGUILayout.EndVertical();
        }



        private void DrawYesNoChoices()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("YES / NO", EditorStyles.boldLabel);

            DrawYesNoChoice("yesChoice", "YES");

            EditorGUILayout.Space();

            DrawYesNoChoice("noChoice", "NO");

            EditorGUILayout.EndVertical();
        }



        private void DrawEndDialogue()
        {
            EditorGUILayout.HelpBox("End dialogue: only lines are displayed. Dialogue closes automatically afterwards.",
                MessageType.Info);
        }



        private void DrawYesNoChoice(string propertyName, string label)
        {
            SerializedProperty choice = serializedObject.FindProperty(propertyName);

            if (choice == null)
                return;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(choice.FindPropertyRelative("nextDialogue"));
            EditorGUILayout.PropertyField(choice.FindPropertyRelative("effects"), true);

            // EditorGUILayout.PropertyField(choice.FindPropertyRelative("onSelected"));

            EditorGUILayout.EndVertical();
        }



        private void DrawAdvanced()
        {
            EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);

            DrawProperty("interruptDialogue");

            DrawProperty("onEnterEffects");

            DrawProperty("onEnter");

            DrawProperty("onExit");
        }



        private void DrawProperty(string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);

            if (property != null)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }



        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Dialogue Node", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(GetHelpText(), MessageType.Info);
        }

        private string GetHelpText()
        {
            return node.dialogueType switch
            {
                DialogueType.Normal => "Normal dialogue: lines followed by choices.",

                DialogueType.YesNo => "Yes/No dialogue: player answers by nodding or shaking.",

                DialogueType.End => "End dialogue: displays lines and closes the conversation.",

                _ =>
                    ""
            };
        }
    }
}

#endif