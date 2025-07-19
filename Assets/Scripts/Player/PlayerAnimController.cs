using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Player
{
    public class PlayerAnimController : MonoBehaviour
    {
        private Animator animator;

        Vector2 mouseWorldPosition;
        Vector2 mouseScreenPosition;
        Vector2 direction;
        Vector2 snappedDIrection;
        Vector2 movementInput;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            movementInput = PlayerController.Instance.MovementInput;
            UpdateMouseDirection();
            UpdateAnimator();
        }

        private void UpdateMouseDirection()
        {
            if (Mouse.current == null)
            {
                Debug.LogWarning(
                    "Mouse input is not available." +
                    "Ensure that the Input System package is installed and configured correctly."
                    );
                return;
            }

            mouseScreenPosition = Mouse.current.position.ReadValue();
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            direction = (mouseWorldPosition - (Vector2)transform.position).normalized;

            snappedDIrection = SnapDIrection(direction);

            animator.SetFloat("lookX", snappedDIrection.x);
            animator.SetFloat("lookY", snappedDIrection.y);
        }

        private void UpdateAnimator()
        {
            animator.SetBool("isMoving", movementInput.magnitude > 0.1f);
        }

        private Vector2 SnapDIrection(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) return new Vector2(Mathf.Sign(direction.x), 0); // left of right
            else return new Vector2(0, Mathf.Sign(direction.y)); // up or down
        }
    }
}
