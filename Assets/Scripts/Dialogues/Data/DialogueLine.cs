using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Character")]
        public Sprite portrait;


        [Header("Text")]
        [TextArea(3, 10)]
        public string text;
    }
}