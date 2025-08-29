using System;
using System.Collections;
using TopDown.Enemy;
using TopDown.Misc;
using TopDown.SceneManagment;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.Player
{
    public class PlayerHealthController : Singleton<PlayerHealthController>
    {
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float invincibleCooldown = 0.8f;
        [Range(0, 1)][SerializeField] private float alphaAmount = 0.6f;

        const string HEALTH_SLIDER_NAME = "HealthSlider";

        private Slider healthSlider;
        private Knockback knockback;
        private Flash flash;
        private SpriteRenderer spriteRenderer;

        private int currentHealth;
        private bool canTakeDamage = true;

        public int MaxHealth { get => maxHealth; set => maxHealth = value; }

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            knockback = GetComponent<Knockback>();
            flash = GetComponent<Flash>();
        }

        void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthSlider();
        }

        public void TakeDamage(int damageAmmount, Transform enemy)
        {
            currentHealth -= damageAmmount;
            DoPlayerTakeDamageSequence(enemy);
            UpdateHealthSlider();

            if (currentHealth <= 0) PlayerDies();
        }

        public void SetPlayerHealthToMax() => currentHealth = maxHealth;

        private void PlayerDies() => StartCoroutine(SpawnManager.Instance.StartPlayerSpawnSequence());


        public void HealPlayer(int healAmmount)
        {
            if (currentHealth >= maxHealth) return;
            currentHealth += healAmmount;
            UpdateHealthSlider();
        }

        public void RestorePlayerAfterDeath()
        {
            currentHealth = maxHealth;
            UpdateHealthSlider();
            Stamina.Instance.RefreshStamina();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

            if (enemy && canTakeDamage)
            {
                TakeDamage(1, enemy.gameObject.transform); // reminder to take care of this 
            }
        }

        private void DoPlayerTakeDamageSequence(Transform damageSource)
        {
            canTakeDamage = false;
            SetAlphaInColor(alphaAmount);
            StartCoroutine(InvincibleCooldownRoutine());
            knockback.GetKnockbacked(damageSource.gameObject.transform);
            flash.TriggerFlash();
            ScreenShaker.Instance.ShakeScreen();

        }

        private void SetAlphaInColor(float value)
        {
            Color temp = spriteRenderer.color;
            temp.a = value;
            spriteRenderer.color = temp;
        }

        private IEnumerator InvincibleCooldownRoutine()
        {
            yield return new WaitForSeconds(invincibleCooldown);
            SetAlphaInColor(1);
            canTakeDamage = true;
        }

        private void UpdateHealthSlider()
        {
            if (healthSlider == null) healthSlider = GameObject.Find(HEALTH_SLIDER_NAME).GetComponent<Slider>();

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
