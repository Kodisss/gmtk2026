using UnityEngine;

namespace Game.Dialogue
{
    public class BossDialogueStarter : MonoBehaviour
    {
        [SerializeField] private DialogueNode startingDialogue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.Instance.StartBossDialogue();
        }
    }
}