using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Dialogue
{
    public class BossDialogueStarter : MonoBehaviour
    {
        [SerializeField] private DialogueNode startingDialogue;
        // [SerializeField] private PlayerInput playerInput;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // playerInput.SwitchCurrentActionMap("BossFight");
            GameManager.Instance.StartBossDialogue();
        }
    }
}