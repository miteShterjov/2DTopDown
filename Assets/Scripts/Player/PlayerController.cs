using System;
using System.Collections;
using TopDown.Misc;
using TopDown.SceneManagment;
using UnityEngine;
using UnityEngine.Video;

namespace TopDown.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
        public bool IsFacingLeft { get => isFacingLeft; set => isFacingLeft = value; }
        public bool CanAttack { get => canAttack; set => canAttack = value; }
        public bool CanMove { get => canMove; set => canMove = value; }

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashCooldown = 0.75f;
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private Transform weaponCollider;
        [SerializeField] private Transform slashAnimSpawner;
        private InputSystem_Actions inputActions;
        private Vector2 movementInput;
        private Rigidbody2D rb;
        private bool isFacingLeft;
        private float defaultMoveSpeed;
        private float dashDuration = .2f;
        private bool isDashing = false;
        private bool canAttack = true;
        private bool canMove = true;

        protected override void Awake()
        {
            base.Awake(); // Call the base Awake method to ensure singleton behavior

            inputActions = new InputSystem_Actions();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            inputActions.Player.Dash.performed += ctx => Dash();
            dashTrail.emitting = false;
            defaultMoveSpeed = moveSpeed;
        }

        void Update()
        {
            MovementInput = inputActions.Player.Move.ReadValue<Vector2>();
            if (canAttack) moveSpeed = defaultMoveSpeed;
        }

        void FixedUpdate()
        {
            PlayerMovement();
        }

        public Transform GetWeaponCollider() { return weaponCollider; }
        public Transform GetSlashAnimSpawner() { return slashAnimSpawner; }

        public void OnEnable()
        {
            inputActions.Enable();
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        private void PlayerMovement()
        {
            if (GetComponent<Knockback>().IsKnockbacked) return;
            rb.MovePosition(rb.position + MovementInput * moveSpeed * Time.fixedDeltaTime);
        }

        private void Dash()
        {
            if (isDashing) return; // Prevent dashing if already dashing
            if (Stamina.Instance.CurrentStamina <= 0) return;
            Stamina.Instance.UseStamina();
            isDashing = true;
            canAttack = false;
            moveSpeed *= dashSpeed;
            dashTrail.emitting = true;
            StartCoroutine(EndDashRoustine());
        }

        private IEnumerator EndDashRoustine()
        {
            yield return new WaitForSeconds(dashDuration);
            moveSpeed /= dashSpeed;
            dashTrail.emitting = false;
            canAttack = true;
            yield return new WaitForSeconds(dashCooldown);
            isDashing = false; // Reset dashing state after cooldown
        }

        internal void DialogueStopMove()
        {
            if (!canAttack) moveSpeed = 0; 
        }
    }
}
