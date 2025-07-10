using System;
using UnityEngine;

namespace TopDown.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private InputSystem_Actions inputActions;
        private Vector2 moveInput;
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            PlayerInput();
        }

        private void FixedUpdate()
        {
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
            if (direction.x < 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
    }
}


