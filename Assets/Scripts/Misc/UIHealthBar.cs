using TopDown.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.Misc
{
    public class UIHealthBar : MonoBehaviour
    {
        public bool IsEnabled { get => isEnabled; set => isEnabled = value; }
        [SerializeField] private Image background;
        [SerializeField] private Image healthBar;
        [SerializeField] float reduceSpeed = 2f;
        private float target = 1f;
        private bool isEnabled;

        void Start()
        {
            background.gameObject.SetActive(false);
        }
        void Update()
        {
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, target, reduceSpeed * Time.deltaTime);
            healthBar.color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);
        }

        public void EnableHealthBar()
        {
            background.gameObject.SetActive(true);
            IsEnabled = true;
        }


        public void UpdateHealthbar(float MaxHealth, float CurrentHealth)
        {
            target = CurrentHealth / MaxHealth;
        }
    }
}
