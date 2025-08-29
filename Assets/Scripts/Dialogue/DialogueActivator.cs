using TopDown.Player;
using UnityEngine;

namespace TopDown.Dialogue
{
    public class DialogueActivator : MonoBehaviour
    {
        [SerializeField] string[] lines;
        [SerializeField] private bool isPerson;
        [SerializeField] private GameObject buttonUI;
        private bool canActivae;
        private InputSystem_Actions inputActions;
        private bool canActivate;

        private const string playerString = "Player";

        void Awake()
        {
            inputActions = new InputSystem_Actions();
        }

        void OnEnable()
        {
            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        void Start()
        {
            inputActions.Player.Interact.performed += _ => OpenDialogue();
        }

        private void OpenDialogue()
        {
            if (canActivate)
            {
                if (!DialogueManager.Instance.dialogueBox.activeInHierarchy)
                {
                    DialogueManager.Instance.ShowDialogue(lines, isPerson);
                    PlayerController.Instance.CanAttack = false;
                    PlayerController.Instance.DialogueStopMove();
                }
                else
                {
                    DialogueManager.Instance.ContinueDialogue();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == playerString)
            {
                buttonUI.SetActive(true);
                canActivate = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == playerString)
            {
                buttonUI.SetActive(false);
                canActivate = false;
            }
        }

    }
}
