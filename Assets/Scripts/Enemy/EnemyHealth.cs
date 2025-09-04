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
        private UIHealthBar healthBar;

        void Awake()
        {
            knockback = GetComponent<Knockback>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Start()
        {
            CurrentHealth = maxHealth;
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
            healthBar.SetHealth(CurrentHealth, MaxHealth);
        }
        void Update()
        {
            if (CurrentHealth <= 0) isDead = true;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            healthBar.SetHealth(CurrentHealth, MaxHealth);
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
