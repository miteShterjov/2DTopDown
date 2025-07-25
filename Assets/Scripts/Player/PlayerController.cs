using System;
using System.Collections;
using TopDown.SceneManagment;
using UnityEngine;
using UnityEngine.Video;

namespace TopDown.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float dashSpeed = 2f;
        [SerializeField] private float dashCooldown = 0.75f;
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private Transform weaponCollider;
        [SerializeField] private Transform slashAnimSpawner;

        private InputSystem_Actions inputActions;
        private Vector2 movementInput;
        private Rigidbody2D rb;
        private bool isFacingLeft;
        
        private float dashDuration = .2f;
        private bool isDashing = false;


        public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
        public bool IsFacingLeft { get => isFacingLeft; set => isFacingLeft = value; }

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
        }

        void Update()
        {
            MovementInput = inputActions.Player.Move.ReadValue<Vector2>();
        }

        void FixedUpdate()
        {
            PlayerMovement();
        }

        public Transform GetWeaponCollider() { return weaponCollider; }
        public Transform GetSlashAnimSpawner() { return slashAnimSpawner; }
            

        private void PlayerMovement()
        {
            rb.MovePosition(rb.position + MovementInput * moveSpeed * Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void Dash()
        {
            if (isDashing) return; // Prevent dashing if already dashing
            isDashing = true;
            moveSpeed *= dashSpeed;
            dashTrail.emitting = true;
            StartCoroutine(EndDashRoustine());
        }

        private IEnumerator EndDashRoustine()
        {
            yield return new WaitForSeconds(dashDuration);
            moveSpeed /= dashSpeed;
            dashTrail.emitting = false;
            yield return new WaitForSeconds(dashCooldown);
            isDashing = false; // Reset dashing state after cooldown
        }
    }
}
