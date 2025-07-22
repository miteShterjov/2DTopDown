using System;
using System.Collections;
using UnityEngine;

namespace TopDown.Misc
{
    public class Flash : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Material flashMaterial;

        private SpriteRenderer spriteRenderer;
        private Material originalMaterial;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            originalMaterial = spriteRenderer.material;
        }

        public void TriggerFlash()
        {
            StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = originalMaterial;
        }
    }
}
