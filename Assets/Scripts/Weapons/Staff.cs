using UnityEngine;

namespace TopDown.Weapons
{
    public class Staff : MonoBehaviour, IWeapon
    {
        [SerializeField] private float attackCooldown = 0.5f; // Example cooldown value
        public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
        public void Attack()
        {
            Debug.Log("Staff Attack");
        }

    }
}
