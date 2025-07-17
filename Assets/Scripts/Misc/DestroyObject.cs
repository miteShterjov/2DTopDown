using UnityEngine;

namespace TopDown.Misc
{
    public class DestroyObject : MonoBehaviour
    {
        [Tooltip("Delay in seconds before destroying the GameObject. If left at 0, it will be destroyed immediately.")]
        [SerializeField] private float delay = 0f;

        void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}
