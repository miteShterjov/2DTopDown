using TopDown.Weapons;
using UnityEngine;

namespace TopDown.Damage
{
    public class DamageSource : MonoBehaviour
    {
        private int damageAmount;
        void Start()
        {
            MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
            damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy.EnemyHealth enemyHealth = collision.GetComponent<Enemy.EnemyHealth>();
                if (enemyHealth != null) enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}
