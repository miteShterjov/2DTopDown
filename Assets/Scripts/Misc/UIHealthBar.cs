using TopDown.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.Misc
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] Vector3 offset;
        [SerializeField] private Color high;
        [SerializeField] private Color low;

        void Update()
        {
            this.slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        }

        public void SetHealth(float currentHealth, float maxHealth)
        {
            slider.gameObject.SetActive(currentHealth < maxHealth);
            slider.value = currentHealth;
            slider.maxValue = maxHealth;

            slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        }
    }
}
