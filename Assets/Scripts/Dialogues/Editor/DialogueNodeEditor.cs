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
            if (node.dialogueType == DialogueType.Normal)
            {
                DrawProperty("choices");
            }
            else
            {
                DrawProperty("yesChoice");

                DrawProperty("noChoice");
            }
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