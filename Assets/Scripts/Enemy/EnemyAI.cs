using System.Collections;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float roamChangeDirectionCounter; //
        [SerializeField] private float attackRange = 0f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private MonoBehaviour enemyType;
        [SerializeField] private bool stopMovingWhileAttacking;

        private bool canAttack = true;
        private enum State
        {
            Roaming,
            Attacking
        }

        private State state;
        private EnemyPathfinder enemyPathfinding;
        private Vector2 roamPosition;
        private float timeRoaming = 0f;

        private void Awake()
        {
            enemyPathfinding = GetComponent<EnemyPathfinder>();
            state = State.Roaming;
        }

        private void Start()
        {
            roamPosition = GetRoamingPosition();
        }

        void Update()
        {
            MovementStateControl();
        }

        private void MovementStateControl()
        {
            switch (state)
            {
                default:
                case State.Roaming:
                    Roaming();
                    print("State is Roaming.");
                    break;

                case State.Attacking:
                    Attacking();
                    print("State is Attacking!");
                    break;
            }
        }

        private void Roaming()
        {
            if (GetComponent<Slime>()) GetComponent<Slime>().IsAttacking = false;
            timeRoaming += Time.deltaTime;
            enemyPathfinding.GetMoveDirection(roamPosition);

            if (AggroCheck()) state = State.Attacking;

            if (timeRoaming > roamChangeDirectionCounter) roamPosition = GetRoamingPosition();

        }

        private void Attacking()
        {
            if (!canAttack && attackRange == 0) return;
            if (!AggroCheck()) state = State.Roaming;
            if (GetComponent<Slime>()) GetComponent<Slime>().IsAttacking = true;
            (enemyType as IEnemy).Attack();
            canAttack = false;

            if (stopMovingWhileAttacking) enemyPathfinding.StopMoving();
            else enemyPathfinding.GetMoveDirection(roamPosition);

            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
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
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
