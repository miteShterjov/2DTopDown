using System;
using UnityEngine;

namespace TopDown.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public bool IsFacingLeft { get => isFacingLeft; set => isFacingLeft = value; }

        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float dashForce = 500f;
        [SerializeField] private float dashDuration = 0.15f;
        [SerializeField] private float dashCooldown = 0.5f;

        private InputSystem_Actions inputActions;
        private Vector2 moveInput;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        
        private bool isFacingLeft = false;
        private bool isDashing = false;
        private float dashCooldownTimer = 0f;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);

            inputActions = new InputSystem_Actions();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            inputActions.Player.Dash.performed += _ => Dash();
            trailRenderer.emitting = false; // Ensure trail is off at start
        }

        void Update()
        {
            PlayerInput();

            // Handle dash cooldown timer
            if (dashCooldownTimer > 0f)
                dashCooldownTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!isDashing)
                MovePlayer();
            AdjustPlayerFacingDiretion();
        }

        private void PlayerInput()
        {
            moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void MovePlayer()
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }

        // Adjust the player's facing direction based on mouse position
        private void AdjustPlayerFacingDiretion()
        {
            //face the player in the direction of the mouse cursor
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Calculate the direction from the player to the mouse position
            Vector2 direction = (mousePosition - rb.position).normalized;
            // flip the player sprite based on the direction
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
                isFacingLeft = true;
            }
            else
            {
                spriteRenderer.flipX = false;
                isFacingLeft = false;
            }
        }

        private void Dash()
        {
            if (!isDashing && dashCooldownTimer <= 0f && moveInput != Vector2.zero)
            {
                StartCoroutine(DashCoroutine());
            }
        }

        private System.Collections.IEnumerator DashCoroutine()
        {
            isDashing = true;
            dashCooldownTimer = dashCooldown;

            rb.linearVelocity = Vector2.zero; // Reset velocity for consistent dash
            trailRenderer.emitting = true; // Enable trail effect
            rb.AddForce(moveInput.normalized * dashForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(dashDuration);

            rb.linearVelocity = Vector2.zero; // Stop dash movement
            trailRenderer.emitting = false; // Disable trail effect
            isDashing = false;
        }
    }
}


