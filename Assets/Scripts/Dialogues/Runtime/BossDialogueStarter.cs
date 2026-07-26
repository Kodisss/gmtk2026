using UnityEngine;

namespace Game.Dialogue
{
    public class BossDialogueStarter : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private ReputationUI reputationDisplay;
        [SerializeField] private DaysLeftUI daysLeftDisplay;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            reputationDisplay.ShowMyself();
            daysLeftDisplay.HideMyself();

            if (player.position.x - transform.position.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
            GameManager.Instance.StartBossDialogue();
        }
    }
}