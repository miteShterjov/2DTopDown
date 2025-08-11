using TopDown.Misc;
using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class GrapeLandSplatter : MonoBehaviour
    {
        [SerializeField] private float colliderDelay = 0.4f;
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
            PlayerHealthController playerHealth = collision.gameObject.GetComponent<PlayerHealthController>();
            playerHealth?.TakeDamage(1, transform);
        }

        private void DisableCollider()
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
}
