using UnityEngine;

namespace Game.Dialogue
{
    public class BossDialogueStarter : MonoBehaviour
    {
        [SerializeField] private DialogueNode startingDialogue;
        [SerializeField] private Transform player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(player.position.x - transform.position.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
            GameManager.Instance.StartBossDialogue();
        }
    }
}