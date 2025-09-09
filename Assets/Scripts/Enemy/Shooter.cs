using System.Collections;
using TopDown.Player;
using TopDown.Weapons;
using UnityEngine;

namespace TopDown.Enemy
{
    public class Shooter : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletMoveSpeed;
        [SerializeField] private int burstCount;
        [SerializeField] private int projectilesPerBurst;
        [SerializeField][Range(0, 359)] private float angleSpread;
        [SerializeField] private float startingDistance = 0.1f;
        [SerializeField] private float timeBetweenBursts;
        [SerializeField] private float restTime = 1f;
        [Tooltip("Fire individual bullet one at the time per burst.")]
        [SerializeField] private bool stagger;
        [Tooltip("Shoot bullets like ping pong balls back and forth between bursts.")]
        [SerializeField] private bool oscillate;
        [SerializeField] private GameObject spawnPrefab;

        private bool isShooting = false;

        private void OnValidate()
        {
            if (oscillate) { stagger = true; }
            if (!oscillate) { stagger = false; }
            if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
            if (burstCount < 1) { burstCount = 1; }
            if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
            if (restTime < 0.1f) { restTime = 0.1f; }
            if (startingDistance < 0.1f) { startingDistance = 0.1f; }
            if (angleSpread == 0) { projectilesPerBurst = 1; }
            if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
        }

        public void BasicConeAttack()
        {
            bulletMoveSpeed = 8f;
            burstCount = 3;
            projectilesPerBurst = 3;
            angleSpread = 60;
            restTime = 3f;
            timeBetweenBursts = 0.75f;
            oscillate = false;
            Attack();
        }

        public void BasicSprayAttack()
        {
            bulletMoveSpeed = 6f;
            burstCount = 5;
            projectilesPerBurst = 5;
            angleSpread = 90;
            restTime = 2f;
            timeBetweenBursts = 0.5f;
            oscillate = true;
            Attack();
        }

        public void SummonGhostAttack()
        {
            int min = 1;
            int max = 4;
            int spawnAmmount = Random.Range(min, max);
            for (int i = 0; i < spawnAmmount; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
                Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
            }
        }

        public void UltimateAttack()
        {
            bulletMoveSpeed = 8;
            burstCount = 4;
            projectilesPerBurst = 16;
            angleSpread = 359;
            timeBetweenBursts = 0.8f;
            oscillate = false;
            Attack();
        }


        public void Attack()
        {
            if (!isShooting) StartCoroutine(ShootRoutine());
        }

        private IEnumerator ShootRoutine()
        {
            isShooting = true;

            float startAngle, currentAngle, angleStep, endAngle;
            float timeBetweenProjectiles = 0f;

            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

            if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }

            for (int i = 0; i < burstCount; i++)
            {
                if (!oscillate)
                {
                    TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
                }

                if (oscillate && i % 2 != 1)
                {
                    TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
                }
                else if (oscillate)
                {
                    currentAngle = endAngle;
                    endAngle = startAngle;
                    startAngle = currentAngle;
                    angleStep *= -1;
                }


                for (int j = 0; j < projectilesPerBurst; j++)
                {
                    Vector2 pos = FindBulletSpawnPos(currentAngle);

                    GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                    newBullet.transform.right = newBullet.transform.position - transform.position;

                    if (newBullet.TryGetComponent(out Projectile projectile)) projectile.UpdateMoveSpeed(bulletMoveSpeed);

                    currentAngle += angleStep;

                    if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
                }

                currentAngle = startAngle;

                if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
            }

            yield return new WaitForSeconds(restTime);
            isShooting = false;
        }

        private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
        {
            Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            startAngle = targetAngle;
            endAngle = targetAngle;
            currentAngle = targetAngle;
            float halfAngleSpread = 0f;
            angleStep = 0;
            if (angleSpread != 0)
            {
                angleStep = angleSpread / (projectilesPerBurst - 1);
                halfAngleSpread = angleSpread / 2f;
                startAngle = targetAngle - halfAngleSpread;
                endAngle = targetAngle + halfAngleSpread;
                currentAngle = startAngle;
            }
        }

        private Vector2 FindBulletSpawnPos(float currentAngle)
        {
            float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

            Vector2 pos = new Vector2(x, y);

            return pos;
        }

    }
}
