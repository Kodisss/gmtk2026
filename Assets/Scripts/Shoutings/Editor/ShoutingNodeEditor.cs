#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ShoutingNode))]
public class ShoutingNodeEditor : Editor
{
    SerializedProperty npcList;
    SerializedProperty lineList;

    private void OnEnable()
    {
        npcList = serializedObject.FindProperty("npcs");
        lineList = serializedObject.FindProperty("lines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawNPCs();

        GUILayout.Space(15);

        DrawLines();

        serializedObject.ApplyModifiedProperties();
    }

    private int GetPositionCount()
    {
        return System.Enum.GetValues(typeof(ShoutingPosition)).Length;
    }

    private bool HasFreePosition()
    {
        HashSet<int> used = new HashSet<int>();

        for (int i = 0; i < npcList.arraySize; i++)
        {
            SerializedProperty position =
                npcList.GetArrayElementAtIndex(i)
                .FindPropertyRelative("position");

            used.Add(position.enumValueIndex);
        }

        return used.Count < GetPositionCount();
    }

    private void DrawNPCs()
    {
        EditorGUILayout.LabelField(
            "NPCs",
            EditorStyles.boldLabel);


        DrawValidationWarnings();


        GUILayout.Space(5);



        for (int i = 0; i < npcList.arraySize; i++)
        {
            SerializedProperty element =
                npcList.GetArrayElementAtIndex(i);


            SerializedProperty npc =
                element.FindPropertyRelative("npc");


            SerializedProperty position =
                element.FindPropertyRelative("position");



            EditorGUILayout.BeginVertical(EditorStyles.helpBox);


            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.PropertyField(
                npc,
                GUIContent.none);



            DrawPositionDropdown(
                position,
                i);



            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                npcList.DeleteArrayElementAtIndex(i);
                break;
            }


            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }


        GUILayout.Space(5);



        GUI.enabled = npcList.arraySize < GetPositionCount() && HasFreePosition();


        if (GUILayout.Button("+ Add NPC"))
        {
            AddNPCWithFreePosition();
        }


        GUI.enabled = true;
    }

    private void AddNPCWithFreePosition()
    {
        int newIndex = npcList.arraySize;

        npcList.InsertArrayElementAtIndex(newIndex);

        SerializedProperty newNPC =
            npcList.GetArrayElementAtIndex(newIndex);


        SerializedProperty position =
            newNPC.FindPropertyRelative("position");


        // Find first free position
        for (int i = 0; i < GetPositionCount(); i++)
        {
            bool used = false;


            for (int j = 0; j < npcList.arraySize - 1; j++)
            {
                SerializedProperty otherPosition =
                    npcList.GetArrayElementAtIndex(j)
                    .FindPropertyRelative("position");


                if (otherPosition.enumValueIndex == i)
                {
                    used = true;
                    break;
                }
            }


            if (!used)
            {
                position.enumValueIndex = i;
                break;
            }
        }
    }



    private void DrawPositionDropdown(
        SerializedProperty position,
        int currentIndex)
    {
        List<string> availablePositions =
            new List<string>();


        List<int> availableIndexes =
            new List<int>();



        for (int i = 0; i < GetPositionCount(); i++)
        {
            bool alreadyUsed = false;


            for (int j = 0; j < npcList.arraySize; j++)
            {
                if (j == currentIndex)
                    continue;


                SerializedProperty other =
                    npcList.GetArrayElementAtIndex(j)
                    .FindPropertyRelative("position");


                if (other.enumValueIndex == i)
                {
                    alreadyUsed = true;
                    break;
                }
            }



            if (!alreadyUsed)
            {
                availablePositions.Add(
                    ((ShoutingPosition)i).ToString());

                availableIndexes.Add(i);
            }
        }



        int currentPosition =
            position.enumValueIndex;



        int displayIndex =
            availableIndexes.IndexOf(
                currentPosition);



        if (displayIndex < 0)
        {
            availablePositions.Insert(
                0,
                ((ShoutingPosition)currentPosition).ToString());

            availableIndexes.Insert(
                0,
                currentPosition);

            displayIndex = 0;
        }



        int newDisplayIndex =
            EditorGUILayout.Popup(
                displayIndex,
                availablePositions.ToArray(),
                GUILayout.Width(120));



        position.enumValueIndex =
            availableIndexes[newDisplayIndex];
    }



    private void DrawLines()
    {
        EditorGUILayout.LabelField(
            "Lines",
            EditorStyles.boldLabel);


        for (int i = 0; i < lineList.arraySize; i++)
        {
            SerializedProperty line =
                lineList.GetArrayElementAtIndex(i);



            EditorGUILayout.BeginVertical(
                EditorStyles.helpBox);



            DrawSpeakerPopup(line);



            EditorGUILayout.PropertyField(
                line.FindPropertyRelative("text"));



            EditorGUILayout.PropertyField(
                line.FindPropertyRelative("typingSpeed"));



            EditorGUILayout.PropertyField(
                line.FindPropertyRelative("voiceVolume"));



            EditorGUILayout.PropertyField(
                line.FindPropertyRelative("waitAfter"));



            if (GUILayout.Button("Remove Line"))
            {
                lineList.DeleteArrayElementAtIndex(i);
                break;
            }



            EditorGUILayout.EndVertical();
        }



        if (GUILayout.Button("+ Add Line"))
        {
            lineList.InsertArrayElementAtIndex(
                lineList.arraySize);
        }
    }



    private void DrawSpeakerPopup(
        SerializedProperty line)
    {
        SerializedProperty speaker =
            line.FindPropertyRelative("speaker");


        List<string> names =
            new List<string>();

        List<ShoutingNPC> npcs =
            new List<ShoutingNPC>();



        for (int i = 0; i < npcList.arraySize; i++)
        {
            SerializedProperty npc =
                npcList.GetArrayElementAtIndex(i)
                .FindPropertyRelative("npc");


            if (npc.objectReferenceValue == null)
                continue;


            ShoutingNPC npcObject =
                npc.objectReferenceValue as ShoutingNPC;


            names.Add(npcObject.npcName);

            npcs.Add(npcObject);
        }



        if (names.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "Add NPCs before creating lines.",
                MessageType.Warning);

            return;
        }



        int index =
            npcs.IndexOf(
                speaker.objectReferenceValue as ShoutingNPC);



        if (index < 0)
            index = 0;



        index =
            EditorGUILayout.Popup(
                "Speaker",
                index,
                names.ToArray());



        speaker.objectReferenceValue =
            npcs[index];
    }



    private void DrawValidationWarnings()
    {
        HashSet<Object> usedNPCs =
            new HashSet<Object>();


        HashSet<int> usedPositions =
            new HashSet<int>();


        for (int i = 0; i < npcList.arraySize; i++)
        {
            SerializedProperty element =
                npcList.GetArrayElementAtIndex(i);


            SerializedProperty npc =
                element.FindPropertyRelative("npc");


            SerializedProperty pos =
                element.FindPropertyRelative("position");



            if (npc.objectReferenceValue != null)
            {
                if (!usedNPCs.Add(npc.objectReferenceValue))
                {
                    EditorGUILayout.HelpBox(
                        "Duplicate NPC detected.",
                        MessageType.Error);
                }
            }



            if (!usedPositions.Add(pos.enumValueIndex))
            {
                EditorGUILayout.HelpBox(
                    "Duplicate position detected.",
                    MessageType.Error);
            }
        }
    }
}

#endif