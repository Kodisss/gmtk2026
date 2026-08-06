using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Visual")]
        [Tooltip("One sprite = static portrait. Multiple sprites = animation.")]
        public Sprite[] portrait;
        [Range(1f, 10f)]
        public int PortraitAnimationFPS = 8;


        [Header("Voice")]
        public AudioClip voiceClip;

        [Range(0.1f, 1f)]
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