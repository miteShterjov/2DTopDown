using UnityEngine;

namespace TopDown.Damage
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy.EnemyHealth enemyHealth = collision.GetComponent<Enemy.EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log($"Enemy took {damageAmount} damage from {gameObject.name}");
                }
            }
        }
    }
}
