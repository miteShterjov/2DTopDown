using System;
using System.Collections;
using TopDown.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapon
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private Transform slashAnimSpawnPoint;
        [SerializeField] private Transform weaponCollider;
        [SerializeField] private float attackCooldown = 1f; // Cooldown duration in seconds

        private InputSystem_Actions inputActions;
        private Animator animator;
        private PlayerController playerController;
        private ActiveWeapon activeWeapon;
        private GameObject slashVFX;
        private bool attackButtonDown, isAttacking = false;

        void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
            if (playerController == null)
            {
                throw new Exception("PlayerController component is required on the same GameObject as Sword.");
            }
            activeWeapon = GetComponentInParent<ActiveWeapon>();
            if (activeWeapon == null)
            {
                throw new Exception("ActiveWeapon component is required on the same GameObject as Sword.");
            }
            inputActions = new InputSystem_Actions();
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            inputActions.Player.Attack.started += _ => StartAttacking();
            inputActions.Player.Attack.canceled += _ => StopAttacking();
        }

        void Update()
        {
            MouseFollowWithOffset();
            Attack();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        public void SwingUpFlipAnim()
        {
            slashVFX.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
            AdjustAnimToPlayerFacing();
        }

        public void FinishAttackAnim()
        {
            weaponCollider.gameObject.SetActive(false);
        }

        private void StartAttacking()
        {
            attackButtonDown = true;
        }

        private void StopAttacking()
        {
            attackButtonDown = false;
        }

        private void Attack()
        {
            if (attackButtonDown && !isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                weaponCollider.gameObject.SetActive(true);
                slashVFX = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
                slashVFX.transform.parent = this.transform.parent;
                AdjustAnimToPlayerFacing();

                StartCoroutine(AttackCDRoutine());
            }
        }

        private void MouseFollowWithOffset()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 playerPosition = Camera.main.WorldToScreenPoint(playerController.transform.position);

            float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, Mathf.Abs(mousePosition.x - playerPosition.x)) * Mathf.Rad2Deg;
            float clampedAngle = Mathf.Clamp(angle, -10f, 25f);

            if (mousePosition.x < playerPosition.x)
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, -180, clampedAngle);
                weaponCollider.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, clampedAngle);
                weaponCollider.localScale = new Vector3(1, 1, 1);
            }
        }

        private void AdjustAnimToPlayerFacing()
        {
            if (playerController.IsFacingLeft)
            {
                slashVFX.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                slashVFX.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private IEnumerator AttackCDRoutine()
        {
            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
    }
}
