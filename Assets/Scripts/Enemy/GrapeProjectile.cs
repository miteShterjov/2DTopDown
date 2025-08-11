using System.Collections;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class GrapeProjectile : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private float heightY = 3f;
        [SerializeField] private GameObject projectileShadow;
        [SerializeField] private GameObject projectileSplatterPrefab;

        void Start()
        {
            GameObject grapeShadow = Instantiate(projectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);

            Vector3 grapeShadowStartPosition = grapeShadow.transform.position;
            Vector3 playerPosition = PlayerController.Instance.transform.position;

            StartCoroutine(ProjectileCurveRoutine(transform.position, playerPosition));
            StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPosition));
        }

        private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
        {
            float timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;

                float linearT = timePassed / duration;
                float heightT = animCurve.Evaluate(linearT);
                float height = Mathf.Lerp(0, heightY, heightT);

                transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

                yield return null;
            }
            
            Instantiate(projectileSplatterPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
        {
            float timePassed = 0f;

            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;

                float linearT = timePassed / duration;
                grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);

                yield return null;
            }

            Destroy(grapeShadow);
        }
    }
}
