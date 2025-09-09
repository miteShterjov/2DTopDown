using System;
using TopDown.Player;
using TopDown.SceneManagment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapons
{
    public class Sword : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private WeaponInfo weaponInfo;
        
        [SerializeField] private float attackCooldown = 1f; // Example cooldown value
        private Animator animator;
        private ActiveWeapon activeWeapon;
        private GameObject slashVFX;
        private Transform slashAnimSpawner;
        private Transform weaponColider;
        private PlayerController playerController;

        public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            activeWeapon = ActiveWeapon.Instance;
            playerController = PlayerController.Instance;
        }

        void Start()
        {
            slashAnimSpawner = playerController.GetSlashAnimSpawner();
            if (slashAnimSpawner == null) Debug.LogError("SlashAnimSpawner not found in Sword's parent.");
            weaponColider = playerController.GetWeaponCollider();
            if (weaponColider == null) Debug.LogError("WeaponCollider not found in Sword's parent.");
        }

        void Update()
        {
            SwordUpdateDirectionAndOffset();
        }

        public void Attack()
        {
            if (Stamina.Instance.CurrentStamina < weaponInfo.attackCost) return;
            Stamina.Instance.UseStamina();
            animator.SetTrigger("Attack");
            weaponColider.gameObject.SetActive(true);
            slashVFX = Instantiate(slashAnimPrefab, slashAnimSpawner.position, slashAnimSpawner.rotation);
            slashVFX.transform.parent = this.transform.parent;
            AudioManager.Instance.PlayPlayerAttackSFX("Sword");
            // print("Attack performed with sword!");
        }

        public void DoneAttackingAnimEvent()
        {
            weaponColider.gameObject.SetActive(false);
        }

        public void SwingFlipAnimEvent()
        {
            slashVFX.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
            AdjustAnimToPlayerFacing();
        }

        public WeaponInfo GetWeaponInfo() { return weaponInfo; }
        
        private void AdjustAnimToPlayerFacing()
        {
            if (PlayerController.Instance.IsFacingLeft)
            {
                slashVFX.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                slashVFX.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void SwordUpdateDirectionAndOffset()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 playerPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

            float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, Mathf.Abs(mousePosition.x - playerPosition.x)) * Mathf.Rad2Deg;
            float clampedAngle = Mathf.Clamp(angle, -10f, 25f);

            if (mousePosition.x < playerPosition.x)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, clampedAngle);
                weaponColider.transform.rotation = Quaternion.Euler(0, -180, 0); // Adjust collider position for left-facing
                weaponColider.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, clampedAngle);
                weaponColider.transform.rotation = Quaternion.Euler(0, 180, 0); // Adjust collider position for right-facing
                weaponColider.localScale = new Vector3(-1, 1, 1);
            }
        }

        
    }
}
