using TopDown.Player;
using UnityEngine;


namespace TopDown.Enemy
{
    public class Slime : MonoBehaviour, IEnemy
    {
        public bool CanSplit { get => canSplit; set => canSplit = value; }
        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

        [SerializeField] private int damage = 1;
        [SerializeField] private int numberOfSapwns = 3;
        [SerializeField] private int spawnsMaxHP = 2;
        [SerializeField] private GameObject slimePrefab;
        [SerializeField] private Material originalMaterial;
        private bool canSplit = true;
        private bool isAttacking = false;
        private EnemyPathfinder pathfinder;
        private Vector3 playerPosition;

        void Awake()
        {
            pathfinder = GetComponent<EnemyPathfinder>();
        }

        void Update()
        {
            playerPosition = PlayerController.Instance.transform.position;
            if (GetComponent<EnemyHealth>().IsDead && canSplit) SpawnSmallerEnemies();
        }

        public void Attack()
        {
            pathfinder.MoveTowardsPlayer(playerPosition); //move towards player
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerHealthController>())
            {
                PlayerHealthController playerHealth = PlayerHealthController.Instance;
                playerHealth.TakeDamage(damage, transform);
                GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }

        private void SpawnSmallerEnemies()
        {

            for (int i = 0; i < numberOfSapwns; i++)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

                GameObject smallEnemy = Instantiate(slimePrefab, transform.position + spawnOffset, Quaternion.identity);

                smallEnemy.transform.localScale = transform.localScale * 0.6f;
                smallEnemy.gameObject.GetComponent<Slime>().CanSplit = false;
                smallEnemy.gameObject.GetComponent<SpriteRenderer>().material = originalMaterial;
                smallEnemy.gameObject.GetComponent<EnemyHealth>().MaxHealth = spawnsMaxHP;
            }
            canSplit = false;
        }
    }
}
