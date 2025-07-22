using UnityEngine;

namespace TopDown.Misc
{
    public class DestroyThis : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 0f;

        public void DestroySelf() => Destroy(gameObject, destroyDelay);
    }
}
