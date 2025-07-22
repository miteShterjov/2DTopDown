using System;
using TopDown.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapons
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private Transform slashAnimSpawner;
        [SerializeField] private Transform weaponColider;
        private InputSystem_Actions inputActions;
        private Animator animator;
        private ActiveWeapon activeWeapon;
        private GameObject slashVFX;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
            animator = GetComponent<Animator>();
            activeWeapon = GetComponentInParent<ActiveWeapon>();
        }

        void Start()
        {
            inputActions.Player.Attack.started += _ => Attack();
        }

        void Update()
        {
            SwordUpdateDirectionAndOffset();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
            inputActions.Player.Attack.started -= _ => Attack();
        }

        private void Attack()
        {
            animator.SetTrigger("Attack");
            weaponColider.gameObject.SetActive(true);
            slashVFX = Instantiate(slashAnimPrefab, slashAnimSpawner.position, slashAnimSpawner.rotation);
            slashVFX.transform.parent = this.transform.parent;
            print("Attack performed with sword!");
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
