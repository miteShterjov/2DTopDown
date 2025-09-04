using System.Collections;
using UnityEngine;

namespace TopDown.Misc
{
    public class Knockback : MonoBehaviour
    {
        public bool IsKnockbacked { get => isKnockbacked; set => isKnockbacked = value; }

        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockbackDuration = 0.5f;
        private Rigidbody2D rb;
        private bool isKnockbacked;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void GetKnockbacked(Transform damageSource)
        {
            isKnockbacked = true;
            rb.linearVelocity = Vector2.zero; // Reset velocity to prevent continuous movement
            Vector2 direction = (transform.position - damageSource.position).normalized * knockbackForce * rb.mass;
            rb.AddForce(direction, ForceMode2D.Impulse);
            StartCoroutine(ResetKnockback());
        }

        private IEnumerator ResetKnockback()
        {
            yield return new WaitForSeconds(knockbackDuration);
            rb.linearVelocity = Vector2.zero; // Stop any movement after knockback
            IsKnockbacked = false;
        }
    }
}
