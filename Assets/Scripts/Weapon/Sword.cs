using System;
using TopDown.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapon
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private GameObject slashAnimPrefab;
        [SerializeField] private Transform slashAnimSpawnPoint;

        private InputSystem_Actions inputActions;
        private Animator animator;
        private PlayerController playerController;
        private ActiveWeapon activeWeapon;

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
            inputActions.Player.Attack.started += _ => Attack();
        }

        void Update()
        {
            MouseFollowWithOffset();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void Attack()
        {
            //fire up the sword animations
            animator.SetTrigger("Attack");
            GameObject slashVFX = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashVFX.transform.parent = this.transform.parent;
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
            }
            else
            {
                activeWeapon.transform.rotation = Quaternion.Euler(0, 0, clampedAngle);
            }
        }
    }
}
