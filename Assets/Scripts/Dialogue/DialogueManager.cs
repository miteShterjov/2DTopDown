using System;
using TMPro;
using TopDown.Player;
using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
       [SerializeField] public GameObject dialogueBox;
        public bool justStarted;

        [SerializeField] private int currentLine;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject nameBox;
        private string[] dialogueLines;
        private const string startsWithSignifierString = "n-";

       public void ContinueDialogue()
        {
            if (!justStarted)
            {
                currentLine++;
                if (currentLine >= dialogueLines.Length)
                {
                    dialogueBox.SetActive(false);
                    justStarted = true;
                    PlayerController.Instance.CanMove = true;
                    PlayerController.Instance.CanAttack = true;
                    EnemiesManager.Instance.ResumeAllEnemies();
                }
                else
                {
                    CheckIfName();
                    dialogueText.text = dialogueLines[currentLine];
                }
            }
            else
            {
                justStarted = false;
            }
        }

        // newLines is passed through from the DialogueActivator class that calls this function
        public void ShowDialogue(string[] newLines, bool isPerson)
        {
            justStarted = true;
            dialogueLines = newLines;
            currentLine = 0;
            CheckIfName();
            dialogueText.text = dialogueLines[currentLine];
            dialogueBox.SetActive(true);
            nameBox.SetActive(isPerson);
            PlayerController.Instance.CanMove = false;
            EnemiesManager.Instance.PauseAllEnemies();
            PlayerController.Instance.CanAttack = false;
            ContinueDialogue();
        }

        // Can signify who's talking in the inspector
        public void CheckIfName()
        {
            if (dialogueLines[currentLine].StartsWith(startsWithSignifierString))
            {
                nameText.text = dialogueLines[currentLine].Replace(startsWithSignifierString, "");
                currentLine++;
            }
        }

     }
}
