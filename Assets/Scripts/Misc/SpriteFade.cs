using System.Collections;
using UnityEngine;

namespace TopDown.Misc
{
    public class SpriteFade : MonoBehaviour
    {
        [SerializeField] private float fadeTime = .4f;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) Debug.LogError("No sprite renderer in SpriteFade");
        }

        public IEnumerator SlowFadeRoutine()
        {
            float elapsedTime = 0;
            float startValue = spriteRenderer.color.a;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
                yield return null;
            }

            Destroy(gameObject);
        }

    }
}
