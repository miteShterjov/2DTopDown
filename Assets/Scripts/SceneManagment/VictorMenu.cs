using System;
using System.Collections.Generic;
using TopDown.Enemy;
using TopDown.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopDown.SceneManagment
{
    public class VictorMenu : Singleton<VictorMenu>
    {
        [SerializeField] private GameObject victorMenuUI; // Reference to the Victor Menu UI
        [SerializeField] private Button newGameButton; // Reference to the Continue Button
        [SerializeField] private Button quitButton; // Reference to the Quit Button
        [SerializeField] private TMPro.TMP_Text helpInfoText; // Reference to the Pause Menu Title Text

        List<string> helpInfoLines = new List<string>()
    {
        "Use WASD or Arrow Keys to move the Player arround the world. Aim with the Mouse and Left Click to shoot your weapon.",
        "Pressing the ESC key will bring up the Pause Menu, where you can choose to continue playing or quit the game.",
        "Use SPACE to dash the player in the direction they are moving.Be careful, as dashing has a cooldown period.",
        "If a NPC has a blue square above their head, you can interact with them by pressing E. To keep the dialoge going press E again.",
        "When you kill enemies they drop stuff. The pick ups are important. Make sure to collect them and be smart how you spent them.",
        "There is no inventory system implemented yet, so pick ups are automatically collected.",
        "Have fun playing my second prototype micro game! If you have any feedback, please comment and let me know.",
        "There is no save system implemented yet, so your progress will not be saved between sessions.",
        "This game is a prototype and is still in development. Expect bugs and unfinished features.",
        "No persistent enemy state manager, so enemies will respawn when you leave and re-enter an area."
    };
        private string sceneToLoad = "Village"; // The scene to load when starting a new game

        void Start()
        {
            DisplayRandomHelpInfo();

            // Assign button listeners
            newGameButton.onClick.AddListener(StartNewGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        void Update()
        {
            //if new scene is scene to load
            if (SceneManager.GetActiveScene().name == sceneToLoad)
            {
                victorMenuUI.SetActive(false);
                PlayerController.Instance.ResetPlayerState();
                Time.timeScale = 1f;
            }
        }

        private void OnEnable()
        {
            BossAI.OnBossDeath += HandleBossDeath;
        }

        private void OnDisable()
        {
            BossAI.OnBossDeath -= HandleBossDeath;
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        public void QuitGame()
        {
            // Quit the application
            Debug.Log("Quitting Game...");
            Application.Quit();

            // If in the editor, stop play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void HandleBossDeath()
        {
            victorMenuUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }

        private void DisplayRandomHelpInfo()
        {
            if (helpInfoLines.Count == 0) return;

            int randomIndex = UnityEngine.Random.Range(0, helpInfoLines.Count);
            helpInfoText.text = helpInfoLines[randomIndex];
        }
    }
}
