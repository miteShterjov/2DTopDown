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
            if (GetComponent<Slime>() != null && GetComponent<Slime>().IsAttacking) return;

            EnemyMove(moveDirection);

            if (moveDirection.x < 0) spriteRenderer.flipX = true;
            else if (moveDirection.x > 0) spriteRenderer.flipX = false;
        }

        public void EnemyMove(Vector2 moveDirection)
        {
            rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }

        public void MoveTowardsPlayer(Vector3 position)
        {
            // Get the direction from enemy to player
            Vector2 direction = (position - transform.position).normalized;

            // Move enemy towards player
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        public void GetMoveDirection(Vector2 targetPosition)
        {
            moveDirection = (targetPosition - rb.position).normalized;
        }

        public void StopMoving() => moveDirection = Vector3.zero;
        

    }
}
