using System.Collections;
using UnityEngine;

namespace TopDown.Misc
{
    public class Flash : MonoBehaviour
    {
        [SerializeField] private Material flashMaterial;
        [SerializeField] private float restoreTime = .2f;

        private Material originalMaterial;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
        }

        public IEnumerator FlashCoroutine()
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(restoreTime);
            spriteRenderer.material = originalMaterial;
        }   
    }
}
