using TopDown.Enemy;
using UnityEngine;

namespace TopDown.SceneManagment
{
    public class EnemiesManager : Singleton<EnemiesManager>
    {
        [SerializeField] private bool gameIsPaused;
        private EnemyAI[] enemies;

        private void Start()
        {
            enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        }

        void Update()
        {
            if (gameIsPaused) PauseAllEnemies();
            else ResumeAllEnemies();
        }

        public void PauseAllEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.IsPaused = true;
            }
        }

        public void ResumeAllEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.IsPaused = false;
            }
        }
    }
}

