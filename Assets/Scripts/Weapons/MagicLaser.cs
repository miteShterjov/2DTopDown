using System;
using System.Collections;
using TopDown.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Weapons
{
    public class MagicLaser : MonoBehaviour
    {
        [SerializeField] private float laserGrowTime = 2f;

        private bool isGrowing = true;

        private float laserRange;
        private SpriteRenderer spriteRenderer;
        private CapsuleCollider2D capsuleCollider2D;


        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        void Start()
        {
            LaserFaceMouse();
        }

        public void UpdateLaserRange(float laserRange)
        {
            this.laserRange = laserRange;
            StartCoroutine(IncreaseLaserLenghtRoutine());
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Indestructuble>() && !collision.isTrigger)
            {
                isGrowing = false;
            }
        }

        private void LaserFaceMouse()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction = (transform.position - mousePosition).normalized;
            transform.right = -direction;
        }

        private IEnumerator IncreaseLaserLenghtRoutine()
        {
            float timePassed = 0;

            while (spriteRenderer.size.x < laserRange && isGrowing)
            {
                timePassed += Time.deltaTime;
                float linearTime = timePassed / laserGrowTime;

                spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), 1f);

                capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), capsuleCollider2D.size.y);
                capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearTime)) / 2, capsuleCollider2D.offset.y);

                yield return null;
            }

            StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
        }
    }
}
