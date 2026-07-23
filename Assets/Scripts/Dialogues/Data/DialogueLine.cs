using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        public Sprite portrait;

        [TextArea(3, 10)]
        public string text;
    }
}