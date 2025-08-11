using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class Grape : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject grapePrefab;

        private Animator animator;
        private SpriteRenderer spriteRenderer;

        readonly int ATTACK_HASH = Animator.StringToHash("attack");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Attack()
        {
            animator.SetTrigger(ATTACK_HASH);

            if (transform.position.x - PlayerController.Instance.transform.position.x < 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
        }

        public void SpawnProjectileAnimEvent()
        {
            Instantiate(grapePrefab, transform.position, Quaternion.identity);
        }
    }
}
