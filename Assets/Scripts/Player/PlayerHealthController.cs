using System;
using System.Collections;
using TopDown.Enemy;
using TopDown.Misc;
using UnityEngine;

namespace TopDown.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float invincibleCooldown = 0.8f;
        [Range(0, 1)][SerializeField] private float alphaAmount = 0.6f;

        private Knockback knockback;
        private Flash flash;
        private SpriteRenderer spriteRenderer;

        private int currentHealth;
        private bool canTakeDamage = true;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            knockback = GetComponent<Knockback>();
            flash = GetComponent<Flash>();
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damageAmmount, Transform enemy)
        {
            currentHealth -= damageAmmount;
            DoPlayerTakeDamageSequence(enemy);

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

    }
}
