using System.Collections;
using UnityEngine;

namespace TopDown.Misc
{
    public class Knockback : MonoBehaviour
    {
        [SerializeField] private float knockbackDuration = 0.5f; // Duration of the knockback effect
        [SerializeField] private float knockbackForce = 4f; // Force applied during knockback
        private Rigidbody2D rb;
        private bool isKnockbacked = false;

        public bool IsKnockbacked { get => isKnockbacked; set => isKnockbacked = value; }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Knockbacked(Transform damageSoruce)
        {
            isKnockbacked = true;
            Vector2 difference = (transform.position - damageSoruce.position).normalized * knockbackForce * rb.mass;
            rb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(KnockbackRoutine());
        }

        private IEnumerator KnockbackRoutine()
        {
            yield return new WaitForSeconds(knockbackDuration);
            rb.linearVelocity = Vector2.zero; // Stop the knockback movement
            isKnockbacked = false;
        }
    }
}
