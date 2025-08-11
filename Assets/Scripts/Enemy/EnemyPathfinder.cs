using TopDown.Misc;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyPathfinder : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Vector2 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (GetComponent<Knockback>().IsKnockbacked) return;

            rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));

            if (moveDirection.x < 0) spriteRenderer.flipX = true;
            else if(moveDirection.x > 0) spriteRenderer.flipX = false;
        }

        public void MoveTo(Vector2 targetPosition)
        {
            moveDirection = (targetPosition - rb.position).normalized;
        }

        public void StopMoving() => moveDirection = Vector3.zero;
        

    }
}
