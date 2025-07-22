using TopDown.Misc;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyPathfinder : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;

        private Rigidbody2D rb;
        private Vector2 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (GetComponent<Knockback>().IsKnockbacked) return;
            
            rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }

        public void MoveTo(Vector2 targetPosition)
        {
            moveDirection = (targetPosition - rb.position).normalized;
        }

    }
}
