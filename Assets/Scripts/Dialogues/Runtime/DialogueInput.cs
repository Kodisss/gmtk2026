using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Dialogue
{
    public class DialogueInput : MonoBehaviour
    {
        private DialogueManager dialogueManager;
        private float lastContinueTime;
        [SerializeField] private float continueCooldown = 0.3f;


        private void Start()
        {
            dialogueManager = transform.GetComponent<DialogueManager>();
        }

        public void OnContinue(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;


            if (Time.time - lastContinueTime < continueCooldown) return;

            lastContinueTime = Time.time;

            dialogueManager.Continue();
        }


        public void OnInterrupt(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            dialogueManager.Interrupt();
        }
    }
}