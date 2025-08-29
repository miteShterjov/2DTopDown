using System;
using TopDown.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

namespace TopDown.Misc
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSpawnSprite;
        [SerializeField] private Sprite activeSpawnSprite;
        [SerializeField] private GameObject fireballBeacon;
        [SerializeField] private Transform playerSpawnPosition;
        public event EventHandler OnSpawnPointActivated;
        private Transform position;
        private bool isActiveSpawnPoint = false;
        private bool wasActiveSpawnPoint = false;
        private SpriteRenderer spriteRenderer;

        public Transform PlayerSpawnPosition { get => playerSpawnPosition; set => playerSpawnPosition = value; }

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                if (!wasActiveSpawnPoint && !isActiveSpawnPoint) EnableActiveSpawnPoint();
            }
        }

        public void EnableActiveSpawnPoint()
        {
            isActiveSpawnPoint = true;
            spriteRenderer.sprite = activeSpawnSprite;
            fireballBeacon.gameObject.SetActive(true);
            OnSpawnPointActivated?.Invoke(this, EventArgs.Empty);
        }

        public void DisableActiveSpawnPoint()
        {
            isActiveSpawnPoint = false;
            wasActiveSpawnPoint = true;
            fireballBeacon.gameObject.SetActive(false);
            spriteRenderer.sprite = defaultSpawnSprite;
        }
    }
}
