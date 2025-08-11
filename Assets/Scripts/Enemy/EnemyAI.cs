using System.Collections;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float roamChangeDirFloat;
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
                    break;

                case State.Attacking:
                    Attacking();
                    break;
            }
        }

        private void Roaming()
        {
            timeRoaming += Time.deltaTime;
            enemyPathfinding.MoveTo(roamPosition);

            if (AggroCheck()) state = State.Attacking;

            if (timeRoaming > roamChangeDirFloat) roamPosition = GetRoamingPosition();

        }

        private void Attacking()
        {
            if (!canAttack && attackRange == 0) return;
            if (!AggroCheck()) state = State.Roaming;
            (enemyType as IEnemy).Attack();
            canAttack = false;

            if (stopMovingWhileAttacking) enemyPathfinding.StopMoving();
            else enemyPathfinding.MoveTo(roamPosition);

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
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        private bool AggroCheck()
        {
            return Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange;
        }

    }
}
