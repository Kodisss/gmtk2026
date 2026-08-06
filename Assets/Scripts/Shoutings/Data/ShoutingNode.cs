using Game.Dialogue;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Shouting Node",
    menuName = "Dialogue/Shouting Node")]
public class ShoutingNode : ScriptableObject
{
    public List<ShoutingNPCInstance> npcs = new();

    public List<ShoutingLine> lines = new();

    private void OnValidate()
    {
        if (lines == null)
            return;

        foreach (ShoutingLine line in lines)
        {
            if (line == null)
                continue;

            if (line.typingSpeed <= 0)
                line.typingSpeed = 1f;


            if (line.voiceVolume <= 0)
                line.voiceVolume = 1f;

            if (line.waitAfter < .5f)
                line.waitAfter = .5f;
        }
    }
}