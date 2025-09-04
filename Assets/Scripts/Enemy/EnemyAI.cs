using System.Collections;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public bool IsPaused { get => isPaused; set => isPaused = value; }
        public bool CanAttack { get => canAttack; set => canAttack = value; }

        [SerializeField] private float roamChangeDirectionCounter; //
        [SerializeField] private float attackRange = 0f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private MonoBehaviour enemyType;
        [SerializeField] private bool stopMovingWhileAttacking;
        [SerializeField] private float waitBeforeNextMove = 1.5f;
        [SerializeField] private float roamingSpeed = 2f;

        private bool canAttack = true;
        private enum State
        {
            Roaming,
            Attacking,
            Paused
        }
        private State state;
        private EnemyPathfinder enemyPathfinding;
        private Vector2 roamPosition;
        private float timeRoaming;
        private bool isPaused;
        private bool isWaiting;
        private SpriteRenderer spriteRenderer;
        private Vector2 roamDirection;

        private void Awake()
        {
            enemyPathfinding = GetComponent<EnemyPathfinder>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            state = State.Roaming;
        }

        private void Start()
        {
            roamPosition = GetRoamingPosition();
            roamDirection = UnityEngine.Random.insideUnitCircle.normalized;
            // Randomize initial roaming timer so enemies desync their movement
            timeRoaming = Random.Range(0f, roamChangeDirectionCounter);
        }

        void Update()
        {
            if (isPaused) state = State.Paused;
            MovementStateControl();
        }

        private void MovementStateControl()
        {
            switch (state)
            {
                default:
                case State.Roaming:
                    Roaming();
                    // print(gameObject + ":State is Roaming.");
                    break;

                case State.Attacking:
                    Attacking();
                    // print(gameObject + ": State is Attacking!");
                    break;

                case State.Paused:
                    GameIsPaused();
                    // print("Game is currently paused!");
                    break;
            }
        }

        private void Roaming()
        {
            // If player is in range, switch to Attacking state
            if (AggroCheck())
            {
                state = State.Attacking;
                return;
            }

            // Roaming logic with wait before moving to next point
            if (!isWaiting)
            {
                timeRoaming -= Time.deltaTime;
                if (timeRoaming <= 0)
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


            // if (GetComponent<Slime>()) GetComponent<Slime>().IsAttacking = false;
            // timeRoaming += Time.deltaTime;
            // enemyPathfinding.GetMoveDirection(roamPosition);

            // if (AggroCheck()) state = State.Attacking;

            // if (timeRoaming > roamChangeDirectionCounter) roamPosition = GetRoamingPosition();
        }
        private IEnumerator WaitBeforeNextMoveCoroutine(float waitTime)
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            roamDirection = UnityEngine.Random.insideUnitCircle.normalized;
            timeRoaming = roamChangeDirectionCounter;
            isWaiting = false;
        }

        private void Attacking()
        {
            if (!CanAttack && attackRange == 0) return;
            if (!AggroCheck()) state = State.Roaming;
            if (GetComponent<Slime>()) GetComponent<Slime>().IsAttacking = true;
            (enemyType as IEnemy).Attack();
            CanAttack = false;

            if (stopMovingWhileAttacking) enemyPathfinding.StopMoving();
            else enemyPathfinding.GetMoveDirection(roamPosition);

            StartCoroutine(AttackCooldownRoutine());
        }

        private void GameIsPaused()
        {
            enemyPathfinding.StopMoving();
            if (!isPaused) state = State.Roaming;
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackCooldown);
            CanAttack = true;
        }

        private Vector2 GetRoamingPosition()
        {
            timeRoaming = 0;
            return new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
        }

        private bool AggroCheck()
        {
            return Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange;
        }

        private void OnDrawGizmos()
        {
            // Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
