using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.Weapons
{
    public class Bow : MonoBehaviour, IWeapon
    {
        [SerializeField] private WeaponInfo weaponInfo;
        [SerializeField] private float attackCooldown = 0.5f; // Example cooldown value
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform arrowSpawnPoint;
        public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
        private Animator animator;
        readonly int FIRE_HASH = Animator.StringToHash("Fire");

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void Attack()
        {
            animator.SetTrigger(FIRE_HASH);
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
            arrow.transform.localScale = new Vector3(-1f * arrow.transform.localScale.x,
                                         arrow.transform.localScale.y,
                                         arrow.transform.localScale.z);
            AudioManager.Instance.PlayPlayerAttackSFX("Bow");
            arrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
        }

        public WeaponInfo GetWeaponInfo()
        {
            return weaponInfo;
        }

    }
}
