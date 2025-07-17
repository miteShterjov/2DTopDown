using TopDown.Weapon;
using UnityEngine;

namespace TopDown.Misc
{
    public class Destructable : MonoBehaviour
    {
        [SerializeField] private GameObject destroyVFX;
        [SerializeField] private float destroyDelay = 0.4f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<DamageSource>() != null)
            {
                Instantiate(destroyVFX, transform.position, Quaternion.identity);
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
