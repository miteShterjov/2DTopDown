using System;
using System.Collections;
using TopDown.Misc;
using TopDown.PickUps;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public bool IsDead { get => isDead; set => isDead = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        [SerializeField] private int maxHealth = 5;
        [SerializeField] private float dieDelay = 0.2f;
        [SerializeField] private ParticleSystem deathVFX;
        [SerializeField] private int currentHealth;
        [SerializeField] private UIHealthBar healthBarPrefab;
        private bool isDead = false;
        private Knockback knockback;
        private SpriteRenderer spriteRenderer;
        private UIHealthBar healthBarInstance;


        void Awake()
        {
            knockback = GetComponent<Knockback>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Start()
        {
            CurrentHealth = maxHealth;
            healthBarInstance = Instantiate(
                healthBarPrefab,
                transform.position + new Vector3(0, 1, 0),
                Quaternion.identity,
                transform
                );
        }
        void Update()
        {
            if (CurrentHealth <= 0) isDead = true;

            // Show health bar when first damaged
            if (currentHealth < maxHealth)
            {
                healthBarInstance.EnableHealthBar();
                if (healthBarInstance.IsEnabled == false) healthBarInstance.UpdateHealthbar(currentHealth, maxHealth);
            }
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            healthBarInstance.UpdateHealthbar(MaxHealth, CurrentHealth);
            DoDamageEyeCandy();

            if (CurrentHealth <= 0) StartCoroutine(WaitKnockbackThenDie());
        }

        private void DoDamageEyeCandy()
        {
            GetComponent<EnemyAI>().CanAttack = false;
            knockback.GetKnockbacked(Player.PlayerController.Instance.transform);
            GetComponent<Flash>().TriggerFlash();
        }

        private IEnumerator WaitKnockbackThenDie()
        {
            yield return new WaitUntil(() => !knockback.IsKnockbacked);
            DoDeathProtocol();
        }

        private void DoDeathProtocol()
        {
            ParticleSystem deathVFXInstance = Instantiate(deathVFX, transform.position, Quaternion.identity);
            deathVFXInstance.transform.SetParent(this.transform);
            spriteRenderer.color = Color.clear; // Make the sprite invisible
            GetComponent<PickUpSpawner>().DropLoot();
            Destroy(gameObject, dieDelay);
        }
    }
}
