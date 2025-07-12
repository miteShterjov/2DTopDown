using UnityEngine;

namespace TopDown.Misc
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private float delay = 0f;

        public void DestroySelf()
        {
            Destroy(gameObject, delay);
        }
    }
}
