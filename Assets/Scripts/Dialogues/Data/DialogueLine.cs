using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Visual")]
        public Sprite portrait;


        [Header("Voice")]
        public AudioClip voiceClip;

        [Range(0f, 1f)]
        public float voiceVolume = 1f;


        [Header("Text")]
        [TextArea(3, 8)]
        public string text;


        [Header("Typing")]
        [Tooltip("1 = normal speed, 2 = twice as fast, 0.5 = slower")]
        [Range(0.1f, 5f)]
        public float typingSpeed = 1f;
    }
}