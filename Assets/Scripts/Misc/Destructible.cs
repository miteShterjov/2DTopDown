using TopDown.Damage;
using TopDown.Weapons;
using UnityEngine;

namespace TopDown.Misc
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] private GameObject destroyVFX;
        [SerializeField] private float dieDelay = 0.2f;

        private GameObject deathVFX;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
            {
                deathVFX = Instantiate(destroyVFX, transform.position, Quaternion.identity);
                deathVFX.transform.SetParent(this.transform);
                GetComponent<SpriteRenderer>().color = Color.clear; // Make the sprite invisible
                Destroy(gameObject, dieDelay);
            }

        }
    }
}
