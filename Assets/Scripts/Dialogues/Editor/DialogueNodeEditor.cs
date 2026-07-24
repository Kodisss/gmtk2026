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

            DrawProperty("lines");


            EditorGUILayout.Space();


            DrawChoices();


            EditorGUILayout.Space();


            DrawAdvanced();


            serializedObject.ApplyModifiedProperties();
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



        private void DrawChoices()
        {
            EditorGUILayout.LabelField("Choice Display", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            if (node.dialogueType == DialogueType.Normal)
            {
                DrawProperty("ChoicePortraits");
                DrawProperty("ChoicePortraitAnimationFPS");
                DrawProperty("choices");
            }
            else
            {
                DrawYesNoChoice("yesChoice", "YES");
                DrawYesNoChoice("noChoice", "NO");
            }
        }


        private void DrawYesNoChoice(string propertyName, string label)
        {
            SerializedProperty choice = serializedObject.FindProperty(propertyName);

            if (choice == null) return;

            SerializedProperty nextDialogue = choice.FindPropertyRelative("nextDialogue");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(nextDialogue);

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



        private string GetHelpText()
        {
            return node.dialogueType switch
            {
                DialogueType.Normal =>
                    "Normal dialogue: lines followed by multiple choices.",

                DialogueType.YesNo =>
                    "Yes / No dialogue: lines followed by two possible answers.",

                _ =>
                    ""
            };
        }
    }
}

#endif