using TopDown.Enemy;
using UnityEngine;

namespace TopDown.Weapon
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<EnemyHealth>())
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
            }
        }
    }
}