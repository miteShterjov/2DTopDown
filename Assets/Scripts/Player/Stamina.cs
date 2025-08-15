using System.Collections;
using TopDown.SceneManagment;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.Player
{
    public class Stamina : Singleton<Stamina>
    {
        public int CurrentStamina { get; private set; }
        [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
        [SerializeField] private int staminaRefreshRate = 3;

        private Transform staminaContainer;
        private int startingStamina = 3;
        private int maxStamina;
        const string STAMINA_CONTAINER = "Stamina Grid";

        protected override void Awake()
        {
            base.Awake();
            maxStamina = startingStamina;
            CurrentStamina = startingStamina;
        }

        void Start()
        {
            staminaContainer = GameObject.Find(STAMINA_CONTAINER).transform;
            if (staminaContainer == null) Debug.LogError("Stamina Container not found");


        }

        public void UseStamina()
        {
            CurrentStamina--;
            UpdateStaminaImages();
        }

        public void RefreshStamina()
        {
            if (CurrentStamina < maxStamina) CurrentStamina++;
            UpdateStaminaImages();
        }

        private void UpdateStaminaImages()
        {
            for (int i = 0; i < maxStamina; i++)
            {
                if (i <= CurrentStamina - 1) staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
                else staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
            }

            if (CurrentStamina < maxStamina)
            {
                StopAllCoroutines();
                StartCoroutine(RefreshStaminaRoutine());
            }
        }

        private IEnumerator RefreshStaminaRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(staminaRefreshRate);
                RefreshStamina();
            }
        }
    }
}
