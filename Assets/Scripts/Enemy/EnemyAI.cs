using System.Collections;
using UnityEngine;

namespace TopDown.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            Roaming,
        }

        private State state;
        private EnemyPathfinding enemyPathfinding;

        private void Awake()
        {
            enemyPathfinding = GetComponent<EnemyPathfinding>();
            state = State.Roaming;
        }

        void Start()
        {
            StartCoroutine(RoamingRoutine());
        }

        private IEnumerator RoamingRoutine()
        {
            while (state == State.Roaming)
            {
                Vector2 roamPosition = GetNewRoamingPosition();
                enemyPathfinding.MoveTowards(roamPosition);
                print($"Roaming to position: {roamPosition}");
                yield return new WaitForSeconds(2f);
            }
        }

        private Vector2 GetNewRoamingPosition()
        {
            return new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
        }
    }
}

