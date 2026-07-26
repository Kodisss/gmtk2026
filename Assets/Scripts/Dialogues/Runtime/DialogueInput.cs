using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Dialogue
{
    public class DialogueInput : MonoBehaviour
    {
        private DialogueManager dialogueManager;
        private float lastContinueTime;
        [SerializeField] private float continueCooldown = 0.3f;

        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private float directionThreshold = 100f;

        [Header("Gesture Detection")]
        [SerializeField] private float movementThreshold = 25f;
        [SerializeField] private float gestureTimeout = 1f;

        private float lastGestureTime;

        private int gestureStep;

        private GestureType currentGesture = GestureType.None;

        private enum GestureType
        {
            None,
            Nod,
            Shake
        }


        private void Start()
        {
            dialogueManager = transform.GetComponent<DialogueManager>();
        }

        private void Update()
        {
            if (!dialogueManager.WaitingForYesNo)
                return;


            Vector2 mouse = Mouse.current.position.ReadValue();


            Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);


            Vector2 direction = mouse - center;


            if (direction.magnitude < directionThreshold)
            {
                dialogueUI.SetReactionDirection(DialogueLookDirection.Center);
                return;
            }


            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    dialogueUI.SetReactionDirection(DialogueLookDirection.Right);
                }
                else
                {
                    dialogueUI.SetReactionDirection(DialogueLookDirection.Left);
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    dialogueUI.SetReactionDirection(DialogueLookDirection.Up);
                }
                else
                {
                    dialogueUI.SetReactionDirection(DialogueLookDirection.Down);
                }
            }
        }

        public void OnContinue(InputAction.CallbackContext context)
        {
            Debug.Log("Try to continue");

            if (!context.performed) return;

            if (Time.time - lastContinueTime < continueCooldown) return;

            lastContinueTime = Time.time;

            dialogueManager.Continue();
        }


        public void OnInterrupt(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            dialogueManager.Interrupt();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!dialogueManager.WaitingForYesNo)
                return;

            Vector2 delta = context.ReadValue<Vector2>();

            if (Time.time - lastGestureTime > gestureTimeout)
            {
                ResetGesture();
            }

            if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                DetectVertical(delta.y);
            }
            else
            {
                DetectHorizontal(delta.x);
            }
        }



        private void DetectVertical(float movement)
        {
            if (Mathf.Abs(movement) < movementThreshold)
                return;

            lastGestureTime = Time.time;

            // Start with UP
            if (gestureStep == 0 && movement > 0)
            {
                currentGesture = GestureType.Nod;
                gestureStep = 1;
                return;
            }

            // DOWN
            if (currentGesture == GestureType.Nod &&
                gestureStep == 1 &&
                movement < 0)
            {
                gestureStep = 2;
                return;
            }

            // UP again
            if (currentGesture == GestureType.Nod &&
                gestureStep == 2 &&
                movement > 0)
            {
                dialogueManager.Yes();
                ResetGesture();
                return;
            }
        }



        private void DetectHorizontal(float movement)
        {
            if (Mathf.Abs(movement) < movementThreshold)
                return;

            lastGestureTime = Time.time;

            // LEFT
            if (gestureStep == 0 && movement < 0)
            {
                currentGesture = GestureType.Shake;
                gestureStep = 1;
                return;
            }

            // RIGHT
            if (currentGesture == GestureType.Shake &&
                gestureStep == 1 &&
                movement > 0)
            {
                gestureStep = 2;
                return;
            }

            // LEFT again
            if (currentGesture == GestureType.Shake &&
                gestureStep == 2 &&
                movement < 0)
            {
                dialogueManager.No();
                ResetGesture();
                return;
            }
        }



        private void ResetGesture()
        {
            currentGesture = GestureType.None;
            gestureStep = 0;
        }
    }
}