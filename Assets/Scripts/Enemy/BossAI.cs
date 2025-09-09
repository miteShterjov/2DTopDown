using System;
using System.Collections;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class BossAI : MonoBehaviour
    {
        public static event Action OnBossDeath; // Event for boss death

        public enum BossState
        {
            Idle,
            Attacking,
            Enraged,
        }

        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 8f;
        // [SerializeField] private float attackCooldown = 2f;

        [Header("Idle Settings")]
        [SerializeField] private float roamingSpeed = 2f;
        [SerializeField] private float roamChangeDirectionCounter = 3f;
        [SerializeField] private float waitBeforeNextMove = 1.5f;

        [Header("Enraged Settings")]
        // [SerializeField] private float enragedDuration = 5f;
        [SerializeField] private GameObject minionPrefab;
        //[SerializeField] private int minionCount = 3;

        private BossState currentState;
        private Vector2 roamDirection;
        private float roamTimer;
        private bool isWaiting = false;
        private SpriteRenderer spriteRenderer;
        private Shooter shooter;
        private float summonTimer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            shooter = GetComponent<Shooter>();
        }

        void Start()
        {
            roamDirection = UnityEngine.Random.insideUnitCircle.normalized;
            roamTimer = roamChangeDirectionCounter;
            currentState = BossState.Idle;
            summonTimer = GetSummonInterval();
        }

        void Update()
        {
            AggroCheck();
            BossStateControl();

            // Handle summoning minions periodically
            summonTimer -= Time.deltaTime;

            if (summonTimer <= 0f)
            {
                if (currentState == BossState.Attacking || currentState == BossState.Enraged)
                {
                    shooter.SummonGhostAttack();
                }
                summonTimer = GetSummonInterval();
            }

            if (GetComponent<EnemyHealth>().CurrentHealth <= 0) OnBossDeath?.Invoke();
        }

        private void BossStateControl()
        {
            switch (currentState)
            {
                case BossState.Idle:
                    HandleIdleState();
                    break;

                case BossState.Attacking:
                    HandleAttackingState();
                    break;

                case BossState.Enraged:
                    HandleEnragedState();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleIdleState()
        {
            // If player is in range, switch to Attacking state
            if (AggroCheck())
            {
                currentState = BossState.Attacking;
                return;
            }

            // Roaming logic with wait before moving to next point
            if (!isWaiting)
            {
                roamTimer -= Time.deltaTime;
                if (roamTimer <= 0)
                {
                    StartCoroutine(WaitBeforeNextMoveCoroutine(waitBeforeNextMove));
                }
            }
            else
            {
                // Don't move while waiting
                return;
            }

            // Move boss in roamDirection
            transform.position += (Vector3)(roamDirection * roamingSpeed * Time.deltaTime);

            // Flip sprite if moving left
            if (roamDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (roamDirection.x > 0)
                spriteRenderer.flipX = false;
        }

        private IEnumerator WaitBeforeNextMoveCoroutine(float waitTime)
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            roamDirection = UnityEngine.Random.insideUnitCircle.normalized;
            roamTimer = roamChangeDirectionCounter;
            isWaiting = false;
        }

        private void HandleAttackingState()
        {
            if ((!AggroCheck()))
            {
                currentState = BossState.Idle;
                return;
            }

            float hpPersentage = (float)GetComponent<EnemyHealth>().CurrentHealth / (float)GetComponent<EnemyHealth>().MaxHealth;

            if (hpPersentage > 0.3f)
            {
                int attackChoice = UnityEngine.Random.Range(0, 2);
                if (attackChoice == 0) shooter.BasicConeAttack();
                if (attackChoice == 1) shooter.BasicSprayAttack();
            }

            if (hpPersentage < 0.3f)
            {
                currentState = BossState.Enraged;
            }

            
        }

        private void HandleEnragedState()
        {
            shooter.UltimateAttack();
        }

        private bool AggroCheck()
        {
            if (!PlayerController.Instance) return false;
            return Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange;
        }

        private float GetSummonInterval()
        {
            float minIndex = 10;
            float maxIndex = 20;
            return UnityEngine.Random.Range(minIndex, maxIndex);
        }

    }
}
