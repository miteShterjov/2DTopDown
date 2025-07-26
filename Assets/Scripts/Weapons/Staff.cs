using System;
using TopDown.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapons
{
    public class Staff : MonoBehaviour, IWeapon
    {
        [SerializeField] private float attackCooldown = 0.5f; // Example cooldown value
        [SerializeField] private WeaponInfo weaponInfo;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private Transform laserSpawnPoint;
        public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

        private Animator animator;
        readonly int FIRE_HASH = Animator.StringToHash("Attack");

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            StaffUpdateDirectionAndOffset();
        }

        public void Attack()
        {
            animator.SetTrigger(FIRE_HASH);
        }

        public void SpawnStaffProjectileAnimEvent()
        {
            GameObject laserBeam = Instantiate(laserPrefab, laserSpawnPoint.position, quaternion.identity);
            laserBeam.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);

        }

        public WeaponInfo GetWeaponInfo() { return weaponInfo; }

        private void StaffUpdateDirectionAndOffset()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 playerPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

            float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, Mathf.Abs(mousePosition.x - playerPosition.x)) * Mathf.Rad2Deg;
            float clampedAngle = Mathf.Clamp(angle, -10f, 25f);

            if (mousePosition.x < playerPosition.x) ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, clampedAngle);
            else ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, clampedAngle);

        }
    }
}
