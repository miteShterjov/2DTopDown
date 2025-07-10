using UnityEngine;

namespace TopDown.Misc
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private float delay = 0f;

        private void Start()
        {
            if (delay <= 0)
            {
                Debug.LogWarning(gameObject.name + " wont be destroyed because delay is set to 0 or less. Change the value");
                return;
            }
            else Destroy(gameObject, delay);
        }
    }
}
