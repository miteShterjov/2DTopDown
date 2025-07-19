using System;
using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private float moveSpeed = 5f;

        private InputSystem_Actions inputActions;
        private Vector2 movementInput;
        private Rigidbody2D rb;

        public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

        protected override void Awake()
        {
            base.Awake(); // Call the base Awake method to ensure singleton behavior

            inputActions = new InputSystem_Actions();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            MovementInput = inputActions.Player.Move.ReadValue<Vector2>();
        }

        void FixedUpdate()
        {
            PlayerMovement();
        }

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


    }
}
