using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyPathfinding : MonoBehaviour
    {
        // takes in a vector2 position and moves the enemy towards that position
        [SerializeField] private float moveSpeed = 3f;

        private Rigidbody2D rb;
        private Vector2 direction;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // This method will be called to move the enemy towards a target position
            // The actual target position should be set by the AI logic
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }

        public void MoveTowards(Vector2 targetPosition)
        {
            direction = (targetPosition - rb.position).normalized;
        }
    }
}
