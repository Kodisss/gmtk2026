using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Dialogue
{
    public class DialogueInput : MonoBehaviour
    {
        private DialogueManager dialogueManager;


        private void Start()
        {
            dialogueManager = transform.GetComponent<DialogueManager>();
        }

        public void OnContinue(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            dialogueManager.Continue();
        }


        public void OnInterrupt(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            dialogueManager.Interrupt();
        }
    }
}