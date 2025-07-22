using System.Collections;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            Roaming
        }

        private State state;
        private EnemyPathfinder enemyPathfinding;

        private void Awake()
        {
            enemyPathfinding = GetComponent<EnemyPathfinder>();
            state = State.Roaming;
        }

        private void Start()
        {
            StartCoroutine(RoamingRoutine());
        }

        private IEnumerator RoamingRoutine()
        {
            while (state == State.Roaming)
            {
                Vector2 roamPosition = GetRoamingPosition();
                enemyPathfinding.MoveTo(roamPosition);
                yield return new WaitForSeconds(2f);
            }
        }

        private Vector2 GetRoamingPosition()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

    }
}
