#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Game.Dialogue.Editor
{
    [CustomEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : UnityEditor.Editor
    {
        private SerializedProperty dialogueType;


        private void OnEnable()
        {
            dialogueType =
                serializedObject.FindProperty(
                    "dialogueType");
        }



        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            DialogueNode node =
                (DialogueNode)target;



            DrawProperty("speakerName");

            DrawProperty("dialogueType");


            DrawProperty("lines");


            if(node.dialogueType == DialogueType.Normal)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField(
                    "Choices",
                    EditorStyles.boldLabel);

                DrawProperty("choices");
            }



            if(node.dialogueType == DialogueType.YesNo)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField(
                    "YES / NO",
                    EditorStyles.boldLabel);


                DrawProperty("yesChoice");

                DrawProperty("noChoice");
            }



            DrawProperty("interruptDialogue");


            DrawProperty("onEnterEffects");


            DrawProperty("onEnter");

            DrawProperty("onExit");



            serializedObject.ApplyModifiedProperties();
        }



        private void DrawProperty(string name)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(name),
                true);
        }



        private string GetHelpText(
            DialogueNode node)
        {
            switch(node.dialogueType)
            {
                case DialogueType.Normal:

                    return
                    "Normal dialogue uses the Choices list.";



                case DialogueType.YesNo:

                    return
                    "Yes/No dialogue uses Yes Choice and No Choice.";



                default:

                    return "";
            }
        }
    }
}

#endif