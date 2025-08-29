using TopDown.Misc;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class GrapeLandSplatter : MonoBehaviour
    {
        [SerializeField] private float colliderDelay = 0.4f;
        [SerializeField] private int damageAmmount = 1;
        private SpriteFade spriteFade;

        private void Awake()
        {
            spriteFade = GetComponent<SpriteFade>();
        }

        private void Start()
        {
            StartCoroutine(spriteFade.SlowFadeRoutine());
            Invoke("DisableCollider", colliderDelay);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerHealthController>())
            {
                print("player detected!");
                PlayerHealthController playerHealth = collision.gameObject.GetComponent<PlayerHealthController>();
                playerHealth?.TakeDamage(damageAmmount, transform);
            }

        }

        private void DisableCollider()
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
}
