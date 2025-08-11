using System;
using TopDown.Enemy;
using TopDown.Misc;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float destroyDelay = 0.8f;
        [SerializeField] private bool isEnemyProjectile = false;
        [SerializeField] private float projectileRange = 10f;
        private Vector3 startPosition;

        private void Start()
        {
            Destroy(gameObject, lifetime);
            startPosition = transform.position;
        }

        private void Update()
        {
            MovePojectile();
            DetectFiredDistance();
        }

        public void UpdateProjectileRange(float projectileRange)
        {
            this.projectileRange = projectileRange;
        }

        public void UpdateMoveSpeed(float moveSpeed)
        {
            this.speed = moveSpeed;
        }

        private void MovePojectile()
        {
            Vector3 moveVector = isEnemyProjectile ? Vector3.right : Vector3.left;
            transform.Translate(moveVector * speed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();

            if ((player && isEnemyProjectile) || (enemy && !isEnemyProjectile))
            {
                // player takes damage
                player?.TakeDamage(1, transform); // handle the magic number when i redo enemies ai 
                RunDestroySequence();
            }
            if (collision.gameObject.GetComponent<Indestructuble>() && !collision.isTrigger) RunDestroySequence();
        }

        private void RunDestroySequence()
        {
            GameObject hitEffectVFX = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            hitEffectVFX.GetComponent<DestroyThis>().DestroySelf();
            Destroy(gameObject);
        }

        private void DetectFiredDistance()
        {
            float distanceTraveled = Vector3.Distance(transform.position, startPosition);
            if (distanceTraveled > projectileRange) Destroy(gameObject);
        }
    }
}
