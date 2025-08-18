using System;
using System.Collections;
using TopDown.Misc;
using TopDown.PickUps;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private float dieDelay = 0.2f;
        [SerializeField] private ParticleSystem deathVFX;

        private int currentHealth;
        private bool isDead = false;

        private Knockback knockback;
        private SpriteRenderer spriteRenderer;

        public bool IsDead { get => isDead; set => isDead = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }

        void Awake()
        {
            knockback = GetComponent<Knockback>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        void Update()
        {
            if (currentHealth <= 0) isDead = true;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            DoDamageEyeCandy();

            if (currentHealth <= 0) StartCoroutine(WaitKnockbackThenDie());
        }

        private void DoDamageEyeCandy()
        {
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
