using System.Collections;
using TopDown.Player;
using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.PickUps
{
    public class PickUps : MonoBehaviour
    {
        private enum PickUpType
        {
            GoldCoin,
            HealthHeart,
            StaminaGlobe
        }

        [SerializeField] private PickUpType pickUpType;
        [SerializeField] private float pickUpDistance = 5f;
        [SerializeField] private float accelerationRate = 0.1f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private float heightY = 1.5f;
        [SerializeField] private float popDuration = 1f;
        [SerializeField] private int healAmmount = 0;

        private Vector3 moveDirection;
        private Rigidbody2D rb;

        private void Start()
        {
            StartCoroutine(AnimCurveSpawnRoutine());
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Vector3 playerPosition = PlayerController.Instance.transform.position;

            if (Vector3.Distance(transform.position, playerPosition) <= pickUpDistance)
            {
                moveDirection = (playerPosition - transform.position).normalized;
                moveSpeed += accelerationRate;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                moveDirection = Vector3.zero;
                moveSpeed = 0f;
            }
        }

        void FixedUpdate()
        {
            rb.linearVelocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DetectPickUpType();
                Destroy(gameObject);
            }
        }

        private void OnValidate()
        {
            if (pickUpType != PickUpType.HealthHeart) healAmmount = 0;
        }

        private IEnumerator AnimCurveSpawnRoutine()
        {
            Vector2 startPoint = transform.position;
            float randomX = transform.position.x + Random.Range(-2f, 2f);
            float randomY = transform.position.y + Random.Range(-1f, 1f);

            Vector2 endPoint = new Vector2(randomX, randomY);

            float timePassed = 0f;

            while (timePassed < popDuration)
            {
                timePassed += Time.deltaTime;
                float linearT = timePassed / popDuration;
                float heightT = animCurve.Evaluate(linearT);
                float height = Mathf.Lerp(0f, heightY, heightT);

                transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
                yield return null;
            }

        }

        private void DetectPickUpType()
        {
            switch (pickUpType)
            {
                case PickUpType.GoldCoin:
                    EconomyManager.Instance.UpdateCurrentGold();
                    break;
                case PickUpType.HealthHeart:
                    PlayerHealthController.Instance.HealPlayer(healAmmount);
                    break;
                case PickUpType.StaminaGlobe:
                    Stamina.Instance.RefreshStamina();
                    break;
            }
        }
    }
}
