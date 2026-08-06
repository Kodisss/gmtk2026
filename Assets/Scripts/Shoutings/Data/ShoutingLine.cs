using UnityEngine;

[System.Serializable]
public class ShoutingLine
{
    [HideInInspector]
    public ShoutingNPC speaker;

    [TextArea(3, 8)]
    public string text;

    [Header("Typing")]
    [Range(0.1f, 5f)]
    public float typingSpeed = 1f;

    [Range(0.1f, 1f)]
    public float voiceVolume = 1f;

    [Range(0.5f, 5f)]
    public float waitAfter = 0.5f;
}