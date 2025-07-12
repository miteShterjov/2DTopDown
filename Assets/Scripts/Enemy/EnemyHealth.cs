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

        private int currentHealth;
        float dieDelay = 0.4f;

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
            Destroy(gameObject, dieDelay);
        }

        private IEnumerator WaitForKnockback()
        {
            yield return new WaitUntil(() => !knockback.IsKnockbacked);
        }
    }
}
