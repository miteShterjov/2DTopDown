using System;
using System.Collections;
using TopDown.Misc;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private GameObject deathVFXPrefab;
        [SerializeField] private bool isDeadRespawn = false;

        private int currentHealth;
        float dieDelay = 0.5f;

        private Knockback knockback;
        private Flash flash;

        void Awake()
        {
            knockback = GetComponent<Knockback>();
            flash = GetComponent<Flash>();
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            knockback.Knockbacked(PlayerController.Instance.transform);
            flash.StartCoroutine(flash.FlashCoroutine());
            print($"Enemy took {damage} damage. Current health: {currentHealth}");

            if (currentHealth <= 0) ExecuteDeathEvent();

        }

        private void ExecuteDeathEvent()
        {
            if (knockback.IsKnockbacked) StartCoroutine(WaitForKnockback());
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            print("DeathVFX instantiated");
            if (gameObject.name == "Enemy_Slime" && !isDeadRespawn)
            {
                isDeadRespawn = true; // Prevent multiple spawns on death
                SpawnSlimes();
            }

            Destroy(gameObject, dieDelay);
        }

        private void SpawnSlimes()
        {
            // the slime enemy before death spawn 2 new slimes with smaller size, 
            // we will scale em down 20%, and set their health to 2
            for (int i = 0; i < 2; i++)
            {
                GameObject newSlime = Instantiate(gameObject, transform.position, Quaternion.identity);
                newSlime.transform.localScale *= 0.8f; // Scale down by 20%
                EnemyHealth newSlimeHealth = newSlime.GetComponent<EnemyHealth>();
                if (newSlimeHealth != null)
                {
                    newSlimeHealth.maxHealth = 2; // Set health to 2
                    newSlimeHealth.currentHealth = 2; // Initialize current health
                    newSlimeHealth.isDeadRespawn = true; // Reset respawn flag
                    newSlime.GetComponent<Flash>().OriginalMaterial = flash.OriginalMaterial; // Copy original material

                }
            }
        }

        private IEnumerator WaitForKnockback()
        {
            yield return new WaitUntil(() => !knockback.IsKnockbacked);
        }
    }
}
