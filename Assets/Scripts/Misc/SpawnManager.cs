using System;
using System.Collections;
using TopDown.Player;
using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.Misc
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        private SpawnPoint activeSpawnPoint;
        private Vector3 currentPlayerSpawnPoint;

        void Start()
        {
            if (activeSpawnPoint == null && PlayerController.Instance != null) currentPlayerSpawnPoint = PlayerController.Instance.transform.position;
        }

        private void OnEnable()
        {
            foreach (SpawnPoint spawnPoint in FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None))
            {
                spawnPoint.OnSpawnPointActivated += SpawnPoint_OnSpawnPointActivated;
            }

            // print("SpawnManager subscribed to all SpawnPoints completed!");
        }

        private void OnDisable()
        {
            foreach (SpawnPoint spawnPoint in FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None))
            {
                spawnPoint.OnSpawnPointActivated -= SpawnPoint_OnSpawnPointActivated;
            }

            // print("SpawnManager unsubscribed from all SpawnPoints.");
        }

        private void SpawnPoint_OnSpawnPointActivated(object sender, System.EventArgs e)
        {
            if (sender is SpawnPoint spawnPoint)
            {
                if (activeSpawnPoint != null) activeSpawnPoint.DisableActiveSpawnPoint();

                activeSpawnPoint = (SpawnPoint)sender;
                currentPlayerSpawnPoint = activeSpawnPoint.PlayerSpawnPosition.transform.position;
            }
        }

        public IEnumerator StartPlayerSpawnSequence()
        {
            yield return StartCoroutine(UI_Fade.Instance.FadeToBlackAndWait());

            EnemiesManager.Instance.PauseAllEnemies();
            PlayerHealthController.Instance.RestorePlayerAfterDeath();
            PlayerController.Instance.gameObject.transform.position = currentPlayerSpawnPoint;

            UI_Fade.Instance.FadeToClear();
            EnemiesManager.Instance.ResumeAllEnemies();
        }

        public void ReLoadSpawnPoints()
        {
            OnDisable();
            OnEnable();
        }
    }
}
